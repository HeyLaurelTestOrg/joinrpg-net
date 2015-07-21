﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JoinRpg.DataModel;

namespace JoinRpg.Data.Interfaces
{
  public interface IProjectRepository : IDisposable
  {
    IEnumerable<Project> AllProjects { get; }
    IEnumerable<Project> ActiveProjects { get; }

    IEnumerable<Project> GetAllMyProjects(int userInfoId);

    IEnumerable<Project> GetMyActiveProjects(int? userInfoId);
    Project GetMyProject(int projectId, int userInfoId);

    Project GetProject(int project);
  }
}