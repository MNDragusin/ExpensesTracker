namespace Client.DTOs;

public class BaseResult
{
    public bool Success { get; set; }
    public string[] Errors { get; set; } = [];
}