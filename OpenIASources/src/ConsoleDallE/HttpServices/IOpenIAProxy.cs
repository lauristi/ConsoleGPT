namespace ConsoleDallE.HttpServices;

public interface IOpenIAProxy
{
    Task<GenerateImageResponse> GenerateImages(GenerateImageRequest prompt, CancellationToken cancellation = default);

    Task<byte[]> DownloadImage(string url);
}
