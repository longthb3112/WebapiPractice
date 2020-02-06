using AutoMapper;
using Microsoft.Web.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using TheCodeCamp.Data;
using TheCodeCamp.Models;

namespace TheCodeCamp.Controllers
{
    [ApiVersion("1.0")]
    [ApiVersion("1.1")]
    [RoutePrefix("api/camps")]
    public class CampsController : ApiController
    {
        private ICampRepository _campsRepository;
        private IMapper _mapper;
        public CampsController(ICampRepository campRepository, IMapper mapper)
        {
            _campsRepository = campRepository;
            _mapper = mapper;
        }
       
        [Route()]
        public async Task<IHttpActionResult> Get(bool includeTalks = false)
        {
            try
            {
                var result = await _campsRepository.GetAllCampsAsync(includeTalks);
                if (result == null) return NotFound();
                //Mapping
                var mapResult = _mapper.Map<IEnumerable<CampModel>>(result);
                return Ok(mapResult);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
               
            }
        }
        //Use ?api-version=1.0 to test
        [MapToApiVersion("1.0")]
        [Route("{moniker}", Name = "GetCamp")]
        public async Task<IHttpActionResult> Get(string moniker)
        {
            try
            {
                var result = await _campsRepository.GetCampAsync(moniker);
                if (result == null) return NotFound();
                //Mapping
                var mapResult = _mapper.Map<CampModel>(result);
                return Ok(mapResult);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);

            }
        }
        [Route("searchByDate/{eventDate:datetime}")]
        [HttpGet]
        public async Task<IHttpActionResult> SearchByDate(DateTime eventDate,bool IncludeTalks = false)
        {
            try
            {
                var result = await _campsRepository.GetAllCampsByEventDate(eventDate, IncludeTalks);
                if (result == null) return NotFound();
                //Mapping
                var mapResult = _mapper.Map<IEnumerable<CampModel>>(result);
                return Ok(mapResult);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);

            }
        }
        [Route()]
        [HttpPost]
        public async Task<IHttpActionResult> Post(CampModel model)
        {
            try
            {
                if (await _campsRepository.GetCampAsync(model.Moniker) != null)
                {
                    ModelState.AddModelError("Moniker", "Moniker In Use");
                }
                if (ModelState.IsValid)
                {
                    //Mapping
                    var mapModel = _mapper.Map<Camp>(model);
                    _campsRepository.AddCamp(mapModel);
                    if (await _campsRepository.SaveChangesAsync())
                    {
                        var newModel = _mapper.Map<CampModel>(mapModel);
                   
                        return CreatedAtRoute("GetCamp", new { moniker = newModel.Moniker }, newModel);
                    }                    
                }

               
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);

            }
            return BadRequest(ModelState);
        }
        [Route("{moniker}")]
        [HttpPut]
        public async Task<IHttpActionResult> Put(string moniker, CampModel model)
        {
            try
            {
                var camp = await _campsRepository.GetCampAsync(moniker);
                if (camp == null) return NotFound();

                _mapper.Map(model, camp);
                if (await _campsRepository.SaveChangesAsync())
                {
                    return Ok(_mapper.Map<CampModel>(camp));
                }
                else
                {
                    return InternalServerError();
                }

            }
            catch (Exception ex)
            {
                return InternalServerError(ex);

            }
            return BadRequest(ModelState);
        }
        [Route("{moniker}")]
        [HttpDelete]
        public async Task<IHttpActionResult> Delete(string moniker)
        {
            try
            {
                var camp = await _campsRepository.GetCampAsync(moniker);
                if (camp == null) return NotFound();

                _campsRepository.DeleteCamp(camp);
                
                if (await _campsRepository.SaveChangesAsync())
                {
                    return Ok();
                }
                else
                {
                    return InternalServerError();
                }

            }
            catch (Exception ex)
            {
                return InternalServerError(ex);

            }
        }
    }
}
