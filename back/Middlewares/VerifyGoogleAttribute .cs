using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Reflection.Emit;
using System.Threading.Tasks;

public class VerifyGoogleAttribute : ActionFilterAttribute
{
    private readonly HttpClient _httpClient;

    public VerifyGoogleAttribute()
    {
        _httpClient = new HttpClient();
    }

    public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (context.HttpContext.Request.Headers.TryGetValue("Authorization", out var authorization))
        {
            var token = authorization.ToString().Replace("Bearer ", "");

            try
            {
                var response = await _httpClient.GetAsync($"https://oauth2.googleapis.com/tokeninfo?access_token={token}");
                if (response.IsSuccessStatusCode)
                {

                    var resultJson = await response.Content.ReadAsStringAsync();
                    var resultObject = JsonConvert.DeserializeObject<dynamic>(resultJson);
                    
                    if (resultObject != null)
                    {                        
                        context.HttpContext.Items["email"] = resultObject.email;
                        await next();
                        return;                        
                    }
                }
            }
            catch (Exception)
            {
                context.Result = new StatusCodeResult((int)HttpStatusCode.InternalServerError);
                return;
            }
        }

        context.Result = new UnauthorizedResult();
    }
}

public class TokenInfoResponse
{
    public string email { get; set; }
}
