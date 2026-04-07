namespace Manage_PocketPc.Services
{
    public interface ICsvImporter 
    {
        Task<int> ImportAllAsync(string folderPath, CancellationToken ct = default);
    }
}
