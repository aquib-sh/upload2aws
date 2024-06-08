using System.Buffers.Text;
using System.Net;
using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

[Route("storage")]
[ApiController]
public class StorageController : ControllerBase
{
    private BucketManager bucketManager;

    public StorageController()
    {
        bucketManager = new BucketManager("geotales.net", Amazon.RegionEndpoint.APSouth1);
    }

    [HttpPost]
    [Route("upload")]
    public async Task<ActionResult> Upload(FileData data, string path="") 
    {
        var encoded = Encoding.UTF8.GetBytes(data.Data);

        var fileContent = Convert.FromBase64String(data.Data);

        string fileName = await bucketManager.Upload(fileContent, data.MimeType, path);

        return Ok(fileName);
    }

    [HttpPost]
    [Route("fetch")]
    public async Task<ActionResult<string>> Fetch(string key) 
    {
        return await bucketManager.GetURL(fileName:key);
    }

    [HttpGet]
    [Route("testme")]
    public ActionResult<string> TestMe()
    {
        return Ok("You are good to go");
    }

}

public class FileData {
    [JsonPropertyName("mimetype")]
    public string MimeType {get; set;}
    
    [JsonPropertyName("data")]
    public string Data {get; set;}
}