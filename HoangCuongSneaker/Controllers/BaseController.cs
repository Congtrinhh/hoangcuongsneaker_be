using HoangCuongSneaker.Core;
using HoangCuongSneaker.Core.Dto;
using HoangCuongSneaker.Core.Dto.Paging;
using HoangCuongSneaker.Core.Model;
using HoangCuongSneaker.Repository;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HoangCuongSneaker.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController<T> : ControllerBase where T : BaseModel
    {
        protected readonly IBaseRepository<T> _baseRepository;
        public BaseController(IBaseRepository<T> baseRepository)
        {
            _baseRepository = baseRepository;
        }
        // GET: api/<BaseController>
        [HttpGet]
        public async Task<ApiResponse> GetAll()
        {
            var response = new ApiResponse();
            try
            {
                List<T> models = await _baseRepository.GetAll();
                response.OnSuccess(data: models);
            }
            catch (Exception e)
            {
                response.OnFailure(e);
            }
            return response;
        }

        // GET api/<BaseController>/5
        [HttpGet("{id}")]
        public async Task<ApiResponse> GetById(int id)
        {
            var response = new ApiResponse();
            try
            {
                T? model = await _baseRepository.Get(id);
                response.OnSuccess(data: model);
            }
            catch (Exception e)
            {
                response.OnFailure(e);
            }
            return response;
        } 

        // POST api/<BaseController>
        [HttpPost]
        public virtual async Task<ApiResponse> Create([FromBody] T model)
        { 
            var response = new ApiResponse();
            try
            {
                T? createdModel = await _baseRepository.Create(model);
                if (createdModel is not null) response.OnSuccess(data: createdModel);
                else throw new Exception("Có lỗi xảy ra");
            }
            catch (Exception e)
            {
                response.OnFailure(e);
            }
            return response;
        }

        // PUT api/<BaseController>/5
        [HttpPut("{id}")]
        public async Task<ApiResponse> Update(int id, [FromBody] T model)
        {
            var response = new ApiResponse();
            try
            {
                model.Id = id;
                T? updatedModel = await _baseRepository.Update(model);
                response.OnSuccess(data: updatedModel);
            }
            catch (Exception e)
            {
                response.OnFailure(e);
            }
            return response;
        }

        // DELETE api/<BaseController>/5
        [HttpDelete("{id}")]
        public async Task<ApiResponse> Delete(int id)
        {
            var response = new ApiResponse();
            try
            {
                int affectedRows = await _baseRepository.Delete(id);
                response.OnSuccess(data: affectedRows);
            }
            catch (Exception e)
            {
                response.OnFailure(e);
            }
            return response;
        }
    }
}
