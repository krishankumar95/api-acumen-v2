using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Logging;

namespace API_Acumen_V2;

public class TracingMiddleware
{
    private RequestDelegate _requestDelegate;
    private List<Trace> _inMemoryTraces = new List<Trace>();

    private ILogger<TracingMiddleware> _logger; 
    public TracingMiddleware(RequestDelegate requestDelegate,ILogger<TracingMiddleware> logger)
    {
        _requestDelegate = requestDelegate;
        _logger = logger;
    }


/*
    HTTP Request: 
    URI
    Method
    HTTP Response: 
    Status Code

    Response Time
*/
    public async Task Invoke(HttpContext context)
    {
        //Catpure Request Parameters
        var trace = new Trace();
        trace.RequestUri = context.Request.GetDisplayUrl();
        trace.Method = context.Request.Method;

        var timekeeper = new Stopwatch();
        timekeeper.Start();
        await _requestDelegate(context);
        timekeeper.Stop();

        trace.ResponseCode = context.Response.StatusCode;
        trace.ResponseTimeInMilliseconds = timekeeper.ElapsedMilliseconds;

        _logger.LogInformation($"Received in a request at URI:{trace.RequestUri} with Method:{trace.Method} ; Response was status code {trace.ResponseCode} and it took time in milliseoncds {trace.ResponseTimeInMilliseconds}");
        _inMemoryTraces.Add(trace);
    }

}
