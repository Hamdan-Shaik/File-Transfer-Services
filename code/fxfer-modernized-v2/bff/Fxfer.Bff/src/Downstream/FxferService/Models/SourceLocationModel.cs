using System;

namespace Fxfer.Bff.Downstream.FxferService.Models;

public sealed class SourceLocationModel
{
    public string? SourceLocationId { get; set; }
    public int ConfigId { get; set; }
    public string ConfigName { get; set; } = string.Empty;
    public string? Description { get; set; }

    public Guid? FtpServerId { get; set; }
    public string? FtpServerName { get; set; }

    public string? Address { get; set; }

    public string? OwnerName { get; set; }
    public string? OwnerEmail { get; set; }
    public Guid OwnerSubsystem { get; set; }
    public string? OwnerPhoneNumber { get; set; }

    public string? Protocol { get; set; }
    public bool IsActive { get; set; }
}