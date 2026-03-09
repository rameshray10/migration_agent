"""
tasks/tasks.py

Individual task builder functions for the migration pipeline.

TOKEN OPTIMISATION CHANGES (vs original):
  - Removed context=[...] chaining — prior task outputs are no longer
    fed wholesale into each new task's context window (that was the primary
    cause of token exhaustion).
  - Prior task output is instead injected as a SHORT summary (<2500 chars)
    directly into the task description string — controlled by CheckpointManager.
  - Verbose inline code templates removed from task_migrate: the Developer
    agent already knows the .NET Core 8 boilerplate; the instructions just
    need to state WHAT to write, not HOW every line of boilerplate looks.
  - Each builder function is called right before the task runs, receiving
    only the prior summary it actually needs.
  - output_file is set on every task so CrewAI also auto-saves the raw
    output as a fallback alongside our CheckpointManager files.

Task execution order (enforced in main.py):
  1. build_analyze_task  → Developer reads legacy project
  2. build_migrate_task  → Developer writes .NET Core 8 project
  3. build_test_task     → Tester writes + runs xUnit tests
  4. build_review_task   → Critic scores the code
  5. build_report_task   → Manager produces final COMPLETE / INCOMPLETE verdict
"""

from crewai import Task


# ──────────────────────────────────────────────────────────────────────────────
# Task 1 — Analyze Legacy Project
# Agent: Developer (read-only analysis pass)
# Prior context needed: none — this is the first task
# ──────────────────────────────────────────────────────────────────────────────

def build_analyze_task(agent, legacy_path: str, output_file: str) -> Task:
    return Task(
        description=f"""
Analyze the legacy ASP.NET MVC project at: {legacy_path}

Steps:
1. list_files on {legacy_path} — get a full file inventory
2. Read Web.config — extract connection strings and app settings
3. Read every .cs file in Controllers/
4. Read every .cs file in Models/
5. Read every .cshtml or .aspx file in Views/
6. Read Global.asax / App_Start/RouteConfig.cs for routing

Produce a Migration Analysis Report with these sections:

## PROJECT SUMMARY
What the app does. Controller count, model count, view count.

## DATABASE
Table names. Connection string format from Web.config.

## CONTROLLERS
Every controller, every action method, HTTP verb, return type.

## VIEWS
Every view file and which model it binds to.

## MIGRATION MAPPING
For each legacy pattern, state the .NET Core 8 equivalent:
  RouteConfig.cs           → Program.cs MapControllerRoute
  Web.config connStrings   → appsettings.json ConnectionStrings
  Html.BeginForm()         → <form asp-action="">
  EF6 DbContext            → EF Core DbContext
  System.Web.Mvc           → Microsoft.AspNetCore.Mvc

## BREAKING CHANGES
Every pattern found that does NOT exist in .NET Core 8.
Include file name and line context for each.
        """,
        expected_output=(
            "A complete Migration Analysis Report with all 6 sections filled in: "
            "PROJECT SUMMARY, DATABASE, CONTROLLERS, VIEWS, MIGRATION MAPPING, BREAKING CHANGES. "
            "Every section must reference actual files and code found in the legacy project."
        ),
        agent=agent,
        output_file=output_file,
    )


# ──────────────────────────────────────────────────────────────────────────────
# Task 2 — Generate .NET Core 8 Project
# Agent: Developer (code generation pass)
# Prior context needed: summary of Task 1 (what models/controllers exist)
# ──────────────────────────────────────────────────────────────────────────────

def build_migrate_task(
    agent,
    legacy_path: str,
    output_path: str,
    prior_analyze_summary: str,
    output_file: str,
) -> Task:
    project_name = output_path.rstrip("/\\").replace("\\", "/").split("/")[-1]

    return Task(
        description=f"""
Generate a complete .NET Core 8 MVC CRUD application.
Output path: {output_path}
Legacy source: {legacy_path}

## ANALYSIS SUMMARY (from Task 1)
{prior_analyze_summary}

## FILES TO WRITE — use write_file for every file below:

1. {output_path}/{project_name}.csproj
   Target net8.0. Include EF Core SqlServer + Tools packages.

2. {output_path}/appsettings.json
   Connection string from the legacy Web.config analysis above.
   Replace real credentials with placeholder values.

3. {output_path}/Program.cs
   Must include in this order:
     builder.Services.AddControllersWithViews()
     builder.Services.AddDbContext<AppDbContext>(...)
     app.UseHttpsRedirection()
     app.UseStaticFiles()
     app.UseRouting()
     app.UseAuthorization()
     app.MapControllerRoute("default", "{{controller=Home}}/{{action=Index}}/{{id?}}")

4. {output_path}/Data/AppDbContext.cs
   EF Core 8. Constructor takes DbContextOptions<AppDbContext>.
   One DbSet<T> for each model found in the legacy project.

5. {output_path}/Models/<ModelName>.cs  (one file per model)
   Clean POCO. Use [Required], [StringLength], [Range] where needed.
   No System.Data or System.Data.Entity imports.

6. {output_path}/Controllers/<Name>Controller.cs  (one per controller)
   Constructor-injected AppDbContext (_context).
   All DB actions must be async (ToListAsync, FindAsync, SaveChangesAsync).
   POST actions must have [ValidateAntiForgeryToken].
   Return NotFound() (not HttpNotFound()), BadRequest() (not HttpStatusCodeResult).
   RedirectToAction("Index") after every successful POST.

7. Views — one folder per controller:
   Index.cshtml    — Bootstrap 5 table, tag helpers (asp-action, asp-route-id)
   Create.cshtml   — <form asp-action="Create">, <input asp-for="">, <span asp-validation-for="">
   Edit.cshtml     — Same as Create, plus hidden <input asp-for="Id">
   Delete.cshtml   — Show item details, confirm with <form asp-action="DeleteConfirmed">
   NO Html.BeginForm(), Html.EditorFor(), Html.ActionLink(), Html.DisplayFor()

8. {output_path}/Views/Shared/_Layout.cshtml
   Bootstrap 5 CDN. Simple nav. @RenderBody(). @await RenderSectionAsync("Scripts", required: false)

9. {output_path}/Views/_ViewImports.cshtml
   @using {project_name}
   @using {project_name}.Models
   @addTagHelper *, Microsoft.AspNetCore.Mvc.TagHelpers

10. {output_path}/Views/_ViewStart.cshtml
    @{{ Layout = "_Layout"; }}

STRICT RULES — zero exceptions:
- Zero System.Web references anywhere
- Zero HttpContext.Current
- No Web.config — only appsettings.json
- All DB calls async
- DbContext via constructor injection only — never new'd up
        """,
        expected_output=(
            "Confirmation that every file was written successfully. "
            "List each file path on its own line with a one-line description of its content."
        ),
        agent=agent,
        output_file=output_file,
    )


# ──────────────────────────────────────────────────────────────────────────────
# Task 3 — Write and Run xUnit Tests
# Agent: Tester
# Prior context needed: summary of Task 2 (which files were written)
# ──────────────────────────────────────────────────────────────────────────────

def build_test_task(
    agent,
    output_path: str,
    prior_migrate_summary: str,
    output_file: str,
) -> Task:
    project_name = output_path.rstrip("/\\").replace("\\", "/").split("/")[-1]

    return Task(
        description=f"""
Write and run xUnit tests for the migrated .NET Core 8 app at: {output_path}

## FILES WRITTEN (from Task 2)
{prior_migrate_summary}

Steps:
1. list_files on {output_path} — confirm the project structure
2. Read the Controller(s) and Model(s)

3. Write the test project file:
   {output_path}.Tests/{project_name}.Tests.csproj
   Required packages:
     xunit (2.6.0+), Microsoft.NET.Test.Sdk, xunit.runner.visualstudio
     Microsoft.EntityFrameworkCore.InMemory
   Must ProjectReference the main app csproj.

4. Write {output_path}.Tests/CrudControllerTests.cs
   Use InMemory database — no real SQL Server required:
     var options = new DbContextOptionsBuilder<AppDbContext>()
         .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
     var context = new AppDbContext(options);
     var controller = new <Name>Controller(context);

   Write exactly these 7 tests:
     [Fact] Index_Returns_ViewResult_With_All_Items()
     [Fact] Create_GET_Returns_Empty_ViewResult()
     [Fact] Create_POST_Valid_Model_Saves_And_Redirects()
     [Fact] Create_POST_Invalid_Model_Returns_View_With_Errors()
     [Fact] Edit_GET_Returns_ViewResult_With_Correct_Item()
     [Fact] Edit_POST_Valid_Model_Updates_And_Redirects()
     [Fact] Delete_POST_Removes_Item_And_Redirects()

5. Run: dotnet test "{output_path}.Tests/"

6. Report PASS/FAIL per test with exact error message for any failure.
        """,
        expected_output=(
            "Test report listing each test name with PASS or FAIL. "
            "For FAIL: exact exception message and line that failed. "
            "Summary line: 'X/7 tests passed'. "
            "Final line: READY FOR REVIEW or NEEDS FIXES "
            "(if NEEDS FIXES: bullet list of what the Developer must change)."
        ),
        agent=agent,
        output_file=output_file,
    )


# ──────────────────────────────────────────────────────────────────────────────
# Task 4 — Code Review
# Agent: Critic
# Prior context needed: none — Critic reads the output files directly via tools
# ──────────────────────────────────────────────────────────────────────────────

def build_review_task(agent, output_path: str, output_file: str) -> Task:
    return Task(
        description=f"""
Review every .cs and .cshtml file in the migrated project at: {output_path}

Use list_files to discover all files, then read each one.

Score checklist — start at 100, deduct for each violation:

Program.cs  (-10 HIGH each):
  - Missing AddControllersWithViews()
  - Missing AddDbContext<AppDbContext>()
  - UseAuthorization() called before UseRouting()
  - No default MapControllerRoute

Controllers (-10 HIGH each):
  - Any using System.Web
  - HttpContext.Current used
  - DbContext newed up instead of injected
  - Synchronous DB calls (SaveChanges / Find / ToList without Async suffix)
  - Missing [ValidateAntiForgeryToken] on POST actions
  - No RedirectToAction after successful POST

Models (-5 MEDIUM each):
  - Missing [Required] on non-nullable string properties
  - Any EF6 / System.Data.Entity references

Views (-5 MEDIUM each):
  - Html.BeginForm() present (should be <form asp-action="">)
  - Html.EditorFor() / Html.TextBoxFor() present (should be <input asp-for="">)
  - Html.ActionLink() present (should be <a asp-action="">)
  - Missing @addTagHelper in _ViewImports.cshtml

appsettings.json (-10 HIGH each):
  - Connection string missing entirely
  - Real password hardcoded (not a placeholder)

.csproj (-5 MEDIUM each):
  - Not targeting net8.0
  - Missing EF Core package references
        """,
        expected_output=(
            "Structured code review: "
            "SCORE: X/100. "
            "HIGH SEVERITY ISSUES (file + description). "
            "MEDIUM SEVERITY ISSUES (file + description). "
            "LOW SEVERITY ISSUES (file + description). "
            "VERDICT: APPROVED (score >= 80) or NEEDS REVISION (score < 80). "
            "If NEEDS REVISION: ordered fix list for the Developer."
        ),
        agent=agent,
        output_file=output_file,
    )


# ──────────────────────────────────────────────────────────────────────────────
# Task 5 — Manager Final Report
# Agent: Manager
# Prior context needed: summaries of Task 3 (tests) and Task 4 (review)
# ──────────────────────────────────────────────────────────────────────────────

def build_report_task(
    agent,
    output_path: str,
    prior_analyze_summary: str,
    prior_migrate_summary: str,
    prior_test_summary: str,
    prior_review_summary: str,
    output_file: str,
) -> Task:
    return Task(
        description=f"""
You are the Migration Project Manager. Evaluate the migration based on the four reports below.

## TASK 1 — ANALYSIS SUMMARY
{prior_analyze_summary}

## TASK 2 — FILES GENERATED SUMMARY
{prior_migrate_summary}

## TASK 3 — TEST RESULTS
{prior_test_summary}

## TASK 4 — CODE REVIEW
{prior_review_summary}

Pass criteria:
  ✅ Code Review Score >= 80/100
  ✅ At least 6/7 tests PASS
  ✅ Core files present: Program.cs, appsettings.json, Controller, Model, Views, DbContext, .csproj

If ALL criteria met → STATUS: COMPLETE
If ANY criteria not met → STATUS: INCOMPLETE
  For INCOMPLETE: list exactly what the Developer must fix, ordered by priority.

Always include: WHAT NEEDS MANUAL HUMAN REVIEW
(Production connection strings, Windows Auth, deployment config,
 any custom HTTP modules, any Session usage)
        """,
        expected_output=(
            "Final Migration Report with: "
            "STATUS: COMPLETE or INCOMPLETE. "
            "MIGRATION SCORE: X% overall. "
            "FILES MIGRATED: full list. "
            "ISSUES REMAINING: list (empty if COMPLETE). "
            "WHAT NEEDS MANUAL HUMAN REVIEW: always present."
        ),
        agent=agent,
        output_file=output_file,
    )
