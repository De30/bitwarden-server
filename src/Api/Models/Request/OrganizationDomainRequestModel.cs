﻿using System.ComponentModel.DataAnnotations;

namespace Bit.Api.Models.Request;

public class OrganizationDomainRequestModel
{
    [Required]
    public Guid OrganizationId { get; set; }

    [Required]
    public string Txt { get; set; }

    [Required]
    public string DomainName { get; set; }
}
