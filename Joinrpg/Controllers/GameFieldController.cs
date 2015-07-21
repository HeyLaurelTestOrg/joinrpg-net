﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JoinRpg.Data.Interfaces;
using JoinRpg.DataModel;
using JoinRpg.Services.Interfaces;
using JoinRpg.Web.Controllers.Common;
using JoinRpg.Web.Models;

namespace JoinRpg.Web.Controllers
{
  [Authorize]
  public class GameFieldController : ControllerGameBase
  {

    private readonly IProjectService _projectService;

    public GameFieldController(ApplicationUserManager userManager, IProjectRepository projectRepository,
      IProjectService projectService) : base(userManager, projectRepository)
    {
      _projectService = projectService;
    }

    private ActionResult InsideField(int projectId, int fieldId,
      Func<Project, ProjectCharacterField, ActionResult> action)
    {
      return InsideProjectSubentity(projectId, fieldId, project => project.AllProjectFields,
        subentity => subentity.ProjectCharacterFieldId, action);
    }


    private ActionResult ReturnToIndex(Project project)
    {
      return RedirectToAction("Index", new {project.ProjectId});
    }

    [HttpGet]
    [Authorize]
    // GET: GameFields
    public ActionResult Index(int projectId)
    {
      return InsideProject(projectId, pa => pa.CanChangeFields, project => View(project));
    }

    // GET: GameFields/Create
    public ActionResult Create(int projectId)
    {
      return InsideProject(projectId, pa => pa.CanChangeFields,
        project => View(new ProjectCharacterField() {ProjectId = projectId}));
    }

    // POST: GameFields/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Create(ProjectCharacterField field)
    {
      return InsideProject(field.ProjectId, pa => pa.CanChangeFields, project =>
      {
        try
        {
          field.IsActive = true;
          _projectService.AddCharacterField(field);

          return ReturnToIndex(project);
        }
        catch
        {
          return View();
        }
      });

    }

    // GET: GameFields/Edit/5
    public ActionResult Edit(int projectId, int projectCharacterFieldId)
    {
      return InsideField(projectId, projectCharacterFieldId, (project, field) =>
      {
        var viewModel = new GameFieldEditViewModel()
        {
          CanPlayerView = field.CanPlayerView,
          CanPlayerEdit = field.CanPlayerEdit,
          FieldHint = field.FieldHint,
          FieldId = field.ProjectCharacterFieldId,
          IsPublic = field.IsPublic,
          Name = field.FieldName,
          ProjectId = field.ProjectId
        };
        return View(viewModel);
      });
    }

    // POST: GameFields/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit(GameFieldEditViewModel viewModel)
    {
      return InsideField(viewModel.ProjectId, viewModel.FieldId, (project, field) =>
      {
        try
        {
          _projectService.UpdateCharacterField(project.ProjectId, field.ProjectCharacterFieldId, viewModel.Name,
            viewModel.FieldHint,
            viewModel.CanPlayerEdit, viewModel.CanPlayerView, viewModel.IsPublic);

          return ReturnToIndex(project);
        }
        catch
        {
          return View(viewModel);
        }
      });
    }

    [HttpGet]
    [Authorize]
    // GET: GameFields/Delete/5
    public ActionResult Delete(int projectId, int projectCharacterFieldId)
    {
      return InsideField(projectId, projectCharacterFieldId, (project, field) => View(field));

    }

    // POST: GameFields/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Delete(int projectId, int projectCharacterFieldId, FormCollection collection)
    {
      return InsideField(projectId, projectCharacterFieldId, (project, field) =>
      {
        try
        {
          _projectService.DeleteField(field.ProjectCharacterFieldId);

          return ReturnToIndex(project);
        }
        catch
        {
          return View(field);
        }
      });


    }
  }
}