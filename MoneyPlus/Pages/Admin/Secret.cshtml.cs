namespace MoneyPlus.Pages.Admin;

[Authorize]
public class SecretModel : PageModel
{
    public string ReturnURL { get; set; }

    public void OnGet(string pwd)
    {
        ReturnURL = Request.Path;

        var url = Request.Path;
        if (string.IsNullOrEmpty(Request.QueryString.Value) || !Request.QueryString.Value.Contains("pouco-segura"))
            RedirectToRoutePermanent("/admin/BlockedPage");

        //if (!string.IsNullOrEmpty(Request.Query["pwd"]) && Request.Query["pwd"] == "pouco-segura")
        //{
        //    // Password is correct, display the protected page content


        //}

        //else
        //{
        //    // Password is incorrect or not present, redirect to an error page or display an error message
        //    Response.Redirect("/admin/BlockedPage");
        //}
    }
}
