using Microsoft.AspNetCore.Mvc;
using MediatR;
using MediatorTaskAPI.Data;

namespace MediatorTaskAPI.Features.Student
{
    //----------------------------------------------------------------Get All Student-----------------------------------------------------------------------------------------------------
    [Route("API/[Controller]")]
    [ApiController]
    public class GetStudentController : Controller
    {
        private readonly IMediator _mediatR;
        public GetStudentController(IMediator mediator)
        {
            _mediatR = mediator;
        }

        [HttpGet]
        public async Task<List<StudentModel>> GetAll()
        {
            return await _mediatR.Send(new GetStudentsQuery());
        }
    }

    public record GetStudentsQuery : IRequest<List<StudentModel>>;

    public class GetStudentsHandler : IRequestHandler<GetStudentsQuery, List<StudentModel>>
    {
        private readonly StudentDbContext _studentDb;
        public GetStudentsHandler(StudentDbContext studentDbContext)
        {
            _studentDb = studentDbContext;
        }
        public async Task<List<StudentModel>> Handle(GetStudentsQuery request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(_studentDb.Students.ToList());
        }
    }

//-------------------------------------------------------------------Add Student--------------------------------------------------------------------------------------------------------
    [Route("API/[Controller]")]
    [ApiController]
    public class PostStudentController : Controller
    {
        private readonly IMediator _mediatR;
        public PostStudentController(IMediator mediator)
        {
            _mediatR = mediator;
        }

        [HttpPost]
        public async Task<StudentModel> AddStudent([FromBody] StudentModel student)
        {
            return await _mediatR.Send(new AddStudentCommand(student));
        }
    }

    public record AddStudentCommand(StudentModel Student) : IRequest<StudentModel>;

    public class AddStudent : IRequestHandler<AddStudentCommand, StudentModel>
    {
        private readonly StudentDbContext _context;
        public AddStudent(StudentDbContext context)
        {
            _context = context;
        }

        public Task<StudentModel> Handle(AddStudentCommand request, CancellationToken cancellationToken)
        {
            _context.Students.Add(request.Student);
            _context.SaveChanges();
            return Task.FromResult(request.Student);
        }
    }

    //---------------------------------------------------------------Get By Id-------------------------------------------------------------------------------------------------------

    [Route("API/[Controller]")]
    [ApiController]
    public class GetStudentByIdController : Controller
    {
        private readonly IMediator _mediatR;
        public GetStudentByIdController(IMediator mediator)
        {
            _mediatR = mediator;
        }

        [HttpGet]
        [Route("{Id:int}")]
        public async Task<StudentModel> GetById([FromRoute] int Id)
        {
            return await _mediatR.Send(new GetStudentById(Id));
        }
    }
    public record GetStudentById(int Id) : IRequest<StudentModel>;

    public class GetByIdHandler : IRequestHandler<GetStudentById, StudentModel>
    {
        private readonly StudentDbContext _context;
        public GetByIdHandler(StudentDbContext context)
        {
            _context = context;
        }

        public Task<StudentModel> Handle(GetStudentById request, CancellationToken cancellationToken)
        {
            var student = _context.Students.FirstOrDefault(x => x.Id == request.Id);
            return Task.FromResult(student);
        }
    }

    //-----------------------------------------------------------------------Update Student-------------------------------------------------------------------------------------------------

    [Route("API/[Controller]")]
    [ApiController]
    public class UpdateStudentController : Controller
    {
        private readonly IMediator _mediatR;
        public UpdateStudentController(IMediator mediator)
        {
            _mediatR = mediator;
        }

        [HttpPut]
        [Route("{Id:int}")]
        public async Task<StudentModel> UpdateStudent([FromRoute] int Id, [FromBody] StudentModel student)
        {
            return await _mediatR.Send(new UpdateStudentCommand(Id, student));
        }
    }

    public record UpdateStudentCommand(int Id, StudentModel Student) : IRequest<StudentModel>;

    public class UpdateStudent : IRequestHandler<UpdateStudentCommand, StudentModel>
    {
        private readonly StudentDbContext _context;
        public UpdateStudent(StudentDbContext context)
        {
            _context = context;
        }

        public Task<StudentModel> Handle(UpdateStudentCommand request, CancellationToken cancellationToken)
        {
            var student = _context.Students.FirstOrDefault(x => x.Id == request.Id);
            if (student != null)
            {
                student.Id = request.Id;
                student.Name = request.Student.Name;
                student.Class = request.Student.Class;
                student.HouseId = request.Student.HouseId;
                _context.SaveChanges();
            }
            return Task.FromResult(student);
        }
    }

    //---------------------------------------------------------------------Delete Student-------------------------------------------------------------------------------------------------

    [Route("API/[Controller]")]
    [ApiController]
    public class DeleteStudentController : Controller
    {
        private readonly IMediator _mediatR;
        public DeleteStudentController(IMediator mediator)
        {
            _mediatR = mediator;
        }
        [HttpDelete]
        [Route("{Id:int}")]
        public async Task<List<StudentModel>> DeleteStudent([FromRoute] int Id)
        {
            return await _mediatR.Send(new DeleteStudentCommand(Id));
        }
    }

    public record DeleteStudentCommand(int Id) : IRequest<List<StudentModel>>;

    public class DeleteStudentHandler : IRequestHandler<DeleteStudentCommand, List<StudentModel>>
    {
        private readonly StudentDbContext _Context;
        public DeleteStudentHandler(StudentDbContext context)
        {
            _Context = context;
        }

        public async Task<List<StudentModel>> Handle(DeleteStudentCommand request, CancellationToken cancellationToken)
        {
            var student = _Context.Students.FirstOrDefault(x => x.Id == request.Id);
            if (student != null)
            {
                _Context.Students.Remove(student);
                _Context.SaveChanges();
            }
            return await Task.FromResult(_Context.Students.ToList());
        }
    }
}