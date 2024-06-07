using AutoMapper;
using DataLayer.Dbos;
using Models;
using Models.Statueses;

namespace DataLayer.Converts
{
  public class DboConverter : IDboConverter
  {
    private readonly IMapper mapper;

    public DboConverter()
    {
      mapper = CreateMapper();
    }

    public TResult Convert<TResult>(object source)
    {
      return mapper.Map<TResult>(source);
    }

    public TResult Update<TSource, TResult>(TSource source, TResult target)
    {
      return mapper.Map(source, target);
    }

    private static IMapper CreateMapper()
    {
      var configuration = new MapperConfiguration(cfg =>
      {
        cfg.ClearPrefixes();
        cfg.AllowNullCollections = false;

        cfg.CreateMap<ChangeDetails, ChangeDetailsDbo>();
        cfg.CreateMap<ChangeDetailsDbo, ChangeDetails>();

        cfg.CreateMap<Comment, CommentDbo>();
        cfg.CreateMap<CommentDbo, Comment>();

        cfg.CreateMap<DataChange, DataChangeDbo>();
        cfg.CreateMap<DataChangeDbo, DataChange>();

        cfg.CreateMap<Project, ProjectDbo>()
              .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.Value));
        cfg.CreateMap<ProjectDbo, Project>()
              .ForMember(d => d.Status, o => o.MapFrom(s => ProjectStatus.FromText(s.Status)));

        cfg.CreateMap<Models.Task, TaskDbo>()
              .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.Value));
        cfg.CreateMap<TaskDbo, Models.Task>()
              .ForMember(d => d.Status, o => o.MapFrom(s => Models.Statueses.TaskStatus.FromText(s.Status)));

        cfg.CreateMap<Team, TeamDbo>();
        cfg.CreateMap<TeamDbo, Team>();

        cfg.CreateMap<User, UserDbo>();
        cfg.CreateMap<UserDbo, User>();

      });
      return configuration.CreateMapper();
    }
  }
}
