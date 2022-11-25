﻿using Bit.Core.Entities;
using Bit.Core.Enums;
using Bit.Core.Models.Business;
using Bit.Core.Models.Data;
using Bit.Core.OrganizationFeatures.Groups.Interfaces;
using Bit.Core.Repositories;
using Bit.Core.Services;

namespace Bit.Core.OrganizationFeatures.Groups;

public class CreateGroupCommand : ICreateGroupCommand
{
    private readonly IEventService _eventService;
    private readonly IGroupRepository _groupRepository;
    private readonly IReferenceEventService _referenceEventService;

    public CreateGroupCommand(
        IEventService eventService,
        IGroupRepository groupRepository,
        IReferenceEventService referenceEventService)
    {
        _eventService = eventService;
        _groupRepository = groupRepository;
        _referenceEventService = referenceEventService;
    }

    public async Task CreateGroupAsync(Group group, Organization organization,
        IEnumerable<SelectionReadOnly> collections = null)
    {
        await GroupRepositoryCreateGroupAsync(group, organization, systemUser: null, collections);
    }

    public async Task CreateGroupAsync(Group group, Organization organization, EventSystemUser systemUser,
        IEnumerable<SelectionReadOnly> collections = null)
    {
        await GroupRepositoryCreateGroupAsync(group, organization, systemUser, collections);
    }

    private async Task GroupRepositoryCreateGroupAsync(Group group, Organization organization, EventSystemUser? systemUser, IEnumerable<SelectionReadOnly> collections = null)
    {
        group.CreationDate = group.RevisionDate = DateTime.UtcNow;

        if (collections == null)
        {
            await _groupRepository.CreateAsync(group);
        }
        else
        {
            await _groupRepository.CreateAsync(group, collections);
        }

        if (systemUser.HasValue)
        {
            await _eventService.LogGroupEventAsync(group, Enums.EventType.Group_Created, systemUser.Value);
        }
        else
        {
            await _eventService.LogGroupEventAsync(group, Enums.EventType.Group_Created);
        }

        await _referenceEventService.RaiseEventAsync(new ReferenceEvent(ReferenceEventType.GroupCreated, organization));
    }
}