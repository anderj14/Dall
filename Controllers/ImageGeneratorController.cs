
using System.ClientModel;
using Microsoft.AspNetCore.Mvc;
using OpenAI.Images;

namespace Dall.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImageGeneratorController : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult> Post(string input)
        {
            try
            {
                var imageClient = new ImageClient("dall-e-3", "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");

                var generateOptions = new ImageGenerationOptions
                {
                    Quality = GeneratedImageQuality.High,
                    Size = GeneratedImageSize.W1024xH1024,
                    Style = GeneratedImageStyle.Natural,
                    ResponseFormat = GeneratedImageFormat.Uri
                };

                var response = await imageClient.GenerateImageAsync(input, generateOptions);

                return Ok(response);
            }
            catch (ClientResultException ex) when (ex.Message.Contains("billing_hard_limit_reached"))
            {
                return StatusCode(402, "Billing limit reached. Please update your OpenAI plan.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }
}