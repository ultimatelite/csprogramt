using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;


#nullable enable
namespace URLExposer
{
  internal class ExposerException : Exception
  {
    public ExposerException(string m)
      : base(m)
    {
    }
  }

  internal class URLExposerMain
  {
    private static HttpClient client = new HttpClient();

    private async Task<string> Expose(string url)
    {
      string respUri = url;
      HttpResponseMessage resp;
      try
      {
        resp = await URLExposerMain.client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
      }
      catch (Exception ex)
      {
        throw new ExposerException("Failed to fetch host, Full Log: \n" + ex.ToString());
      }
      HttpRequestMessage uriHeader = resp.RequestMessage ?? throw new ExposerException("Header of data is null.");
      Uri uriReq = uriHeader.RequestUri ?? throw new ExposerException("url is missing from HTTP 1.1 request.");
      respUri = uriReq.ToString();
      string str = respUri;
      respUri = (string) null;
      resp = (HttpResponseMessage) null;
      uriHeader = (HttpRequestMessage) null;
      uriReq = (Uri) null;
      return str;
    }

    public static async Task Main(string[] args)
    {
      try
      {
        if (args.Length < 1)
          throw new ExposerException("Failed to retreive host due to args.position(1) is null.");
        if (string.IsNullOrWhiteSpace(args[0]))
          throw new ExposerException("Field of args.position(1) is empty.");
        if (args.Length == 2)
        {
          if (args[1].ToLower().Equals("use_unencrypted_transfer"))
          {
            Console.WriteLine("Performing operation with unencrypted transfer...");
            string checkS = args[0].Contains("http://") ? args[0] : "http://" + args[0];
            string str = await new URLExposerMain().Expose(checkS);
            Console.WriteLine(string.Format("Final URL: {0}", (object) str));
            str = (string) null;
            checkS = (string) null;
          }
          else
          {
            Console.WriteLine("Performing operation with encrypted transfer...");
            string check = args[0].Contains("https://") ? args[0] : "https://" + args[0];
            string str = await new URLExposerMain().Expose(check);
            Console.WriteLine(string.Format("Final URL: {0}", (object) str));
            str = (string) null;
            check = (string) null;
          }
        }
        else
        {
          if (args.Length != 1)
            return;
          Console.WriteLine("Performing operation with encrypted transfer...");
          string check = args[0].Contains("https://") ? args[0] : "https://" + args[0];
          string str = await new URLExposerMain().Expose(check);
          Console.WriteLine(string.Format("Final URL: {0}", (object) str));
          str = (string) null;
          check = (string) null;
        }
      }
      catch (ExposerException ex)
      {
        Console.WriteLine("Failed to execute program properly due to exception being thrown, exiting program and logging error messages...");
        Thread.Sleep(3000);
        Console.Error.WriteLine(ex.ToString());
        Environment.Exit(1);
      }
    }
  }
}
