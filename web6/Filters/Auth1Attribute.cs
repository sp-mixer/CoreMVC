using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public class Auth1Attribute : ActionFilterAttribute {
    public override void OnActionExecuting(ActionExecutingContext context) {
        var session = context.HttpContext.Session;
        var auth1 = session.GetString("Auth1");

        if (auth1 != "True" && auth1 != "1") {
            // 権限なし → ホーム画面へ
            context.Result = new RedirectResult("/Home/Index");
        }
    }
}
