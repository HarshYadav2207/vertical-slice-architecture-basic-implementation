using Microsoft.AspNetCore.Mvc;
using MediatR;
using MediatorTaskAPI.Data;

namespace MediatorTaskAPI.Features.House
{
    //----------------------------------------------------------------Get All Student-----------------------------------------------------------------------------------------------------
    [Route("API/[Controller]")]
    [ApiController]
    public class GetHousesController : Controller
    {
        private readonly IMediator _mediatR;
        public GetHousesController(IMediator mediator)
        {
            _mediatR = mediator;
        }

        [HttpGet]
        public async Task<List<HouseModel>> GetAll()
        {
            return await _mediatR.Send(new GetHousesQuery());
        }
    }

    public record GetHousesQuery : IRequest<List<HouseModel>>;

    public class GetStudentsHandler : IRequestHandler<GetHousesQuery, List<HouseModel>>
    {
        private readonly StudentDbContext _Db;
        public GetStudentsHandler(StudentDbContext studentDbContext)
        {
            _Db = studentDbContext;
        }
        public async Task<List<HouseModel>> Handle(GetHousesQuery request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(_Db.houses.ToList());
        }
    }

//-------------------------------------------------------------------Add Student--------------------------------------------------------------------------------------------------------
    [Route("API/[Controller]")]
    [ApiController]
    public class PostHouseController : Controller
    {
        private readonly IMediator _mediatR;
        public PostHouseController(IMediator mediator)
        {
            _mediatR = mediator;
        }

        [HttpPost]
        public async Task<HouseModel> AddHouse([FromBody] HouseModel house)
        {
            return await _mediatR.Send(new AddHouseCommand(house));
        }
    }

    public record AddHouseCommand(HouseModel house) : IRequest<HouseModel>;

    public class AddStudent : IRequestHandler<AddHouseCommand, HouseModel>
    {
        private readonly StudentDbContext _context;
        public AddStudent(StudentDbContext context)
        {
            _context = context;
        }

        public Task<HouseModel> Handle(AddHouseCommand request, CancellationToken cancellationToken)
        {
            _context.houses.Add(request.house);
            _context.SaveChanges();
            return Task.FromResult(request.house);
        }
    }

    //---------------------------------------------------------------Get By Id-------------------------------------------------------------------------------------------------------

    [Route("API/[Controller]")]
    [ApiController]
    public class GetHouseByIdController : Controller
    {
        private readonly IMediator _mediatR;
        public GetHouseByIdController(IMediator mediator)
        {
            _mediatR = mediator;
        }

        [HttpGet]
        [Route("{Id:int}")]
        public async Task<HouseModel> GetById([FromRoute] int Id)
        {
            return await _mediatR.Send(new GetHouseById(Id));
        }
    }
    public record GetHouseById(int Id) : IRequest<HouseModel>;

    public class GetByIdHandler : IRequestHandler<GetHouseById, HouseModel>
    {
        private readonly StudentDbContext _context;
        public GetByIdHandler(StudentDbContext context)
        {
            _context = context;
        }

        public Task<HouseModel> Handle(GetHouseById request, CancellationToken cancellationToken)
        {
            var house = _context.houses.FirstOrDefault(x => x.Id == request.Id);
            return Task.FromResult(house);
        }
    }

    //-----------------------------------------------------------------------Update Student-------------------------------------------------------------------------------------------------

    [Route("API/[Controller]")]
    [ApiController]
    public class UpdateHouseController : Controller
    {
        private readonly IMediator _mediatR;
        public UpdateHouseController(IMediator mediator)
        {
            _mediatR = mediator;
        }

        [HttpPut]
        [Route("{Id:int}")]
        public async Task<HouseModel> UpdateHouse([FromRoute] int Id, [FromBody] HouseModel house)
        {
            return await _mediatR.Send(new UpdateHouseCommand(Id, house));
        }
    }

    public record UpdateHouseCommand(int Id, HouseModel house) : IRequest<HouseModel>;

    public class UpdateHouse : IRequestHandler<UpdateHouseCommand, HouseModel>
    {
        private readonly StudentDbContext _context;
        public UpdateHouse(StudentDbContext context)
        {
            _context = context;
        }

        public Task<HouseModel> Handle(UpdateHouseCommand request, CancellationToken cancellationToken)
        {
            var house = _context.houses.FirstOrDefault(x => x.Id == request.Id);
            if (house != null)
            {
                house.Id = request.Id;
                house.HouseName = request.house.HouseName;

                _context.SaveChanges();
            }
            return Task.FromResult(house);
        }
    }

    //---------------------------------------------------------------------Delete Student-------------------------------------------------------------------------------------------------

    [Route("API/[Controller]")]
    [ApiController]
    public class DeleteHouseController : Controller
    {
        private readonly IMediator _mediatR;
        public DeleteHouseController(IMediator mediator)
        {
            _mediatR = mediator;
        }
        [HttpDelete]
        [Route("{Id:int}")]
        public async Task<List<HouseModel>> DeleteHouse([FromRoute] int Id)
        {
            return await _mediatR.Send(new DeleteHouseCommand(Id));
        }
    }

    public record DeleteHouseCommand(int Id) : IRequest<List<HouseModel>>;

    public class DeleteStudentHandler : IRequestHandler<DeleteHouseCommand, List<HouseModel>>
    {
        private readonly StudentDbContext _Context;
        public DeleteStudentHandler(StudentDbContext context)
        {
            _Context = context;
        }

        public async Task<List<HouseModel>> Handle(DeleteHouseCommand request, CancellationToken cancellationToken)
        {
            var house = _Context.houses.FirstOrDefault(x => x.Id == request.Id);
            if (house != null)
            {
                _Context.houses.Remove(house);
                _Context.SaveChanges();
            }
            return await Task.FromResult(_Context.houses.ToList());
        }
    }
}