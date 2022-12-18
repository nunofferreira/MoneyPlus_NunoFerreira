namespace MoneyPlus.Pages.Admin;

[Authorize]
public class SecretModel : PageModel
{
    public string ReturnURL { get; set; }

    public void OnGet(string pwd)
    {
        ReturnURL = Request.Path;

        if (!string.IsNullOrEmpty(Request.Query["pwd"]) && Request.Query["pwd"] == "pouco-segura") ;
        else
            Response.Redirect("/admin/BlockedPage");
    }
}