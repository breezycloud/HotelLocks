using ProRFL.UI.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace ProRFL.UI.Handlers;

public class CustomAuthorizationHandler : DelegatingHandler
{    
    public CustomAuthorizationHandler()
    {        
    }
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        //getting token from the localpath
        var jwtToken = await File.ReadAllTextAsync(AppSetting.Token!);

        //adding the token in authorization header
        if (jwtToken != null)
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);

        //sending the request
        return await base.SendAsync(request, cancellationToken);
    }
}
