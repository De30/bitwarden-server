﻿using System.ComponentModel.DataAnnotations;
using Bit.Core.Utilities;

namespace Bit.Core.Entities;

public class OrganizationDomain : ITableObject<Guid>
{
    public Guid Id { get; set; }
    public Guid OrganizationId { get; set; }
    public string Txt { get; set; }
    [MaxLength(255)]
    public string DomainName { get; set; }
    public DateTime CreationDate { get; set; } = DateTime.UtcNow;
    public DateTime? VerifiedDate { get; set; }
    public DateTime NextRunDate { get; private set; }
    public int NextRunCount { get; private set; }
    public void SetNewId() => Id = CoreHelpers.GenerateComb();

    public OrganizationDomain SetNextRunDate()
    {
        if (NextRunCount < 1)
        {
            //throw exception;
        }

        NextRunDate = CreationDate.AddHours(NextRunCount * 12);
        return this;
    }

    public OrganizationDomain SetNextRunCount(int nextRunCount)
    {
        if (nextRunCount == 3)
        {
            return this;
            //or throw exception
        }

        nextRunCount++;
        NextRunCount = nextRunCount;
        return this;
    }
}
