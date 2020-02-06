using AutoMapper;
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
    [RoutePrefix("api/camps/{moniker}/talks")]
    public class TalksController : ApiController
    {
        private ICampRepository _campsRepository;
        private IMapper _mapper;
        public TalksController(ICampRepository campRepository, IMapper mapper)
        {
            _campsRepository = campRepository;
            _mapper = mapper;
        }
       /// <summary>
       /// find all talks by moniker
       /// </summary>
       /// <param name="moniker"></param>
       /// <param name="includeSpeakers"></param>
       /// <returns></returns>
        [Route()]
        public async Task<IHttpActionResult> Get(string moniker, bool includeSpeakers = false)
        {
            try
            {
                var result = await _campsRepository.GetTalksByMonikerAsync(moniker, includeSpeakers);
                if (result == null) return NotFound();
                //Mapping
                var mapResult = _mapper.Map<IEnumerable<TalkModel>>(result);
                return Ok(mapResult);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
               
            }
        }
        /// <summary>
        /// Find talk by talk id inside camp 
        /// </summary>
        /// <param name="moniker"></param>
        /// <param name="id"></param>
        /// <param name="includeSpeakers"></param>
        /// <returns></returns>
        [Route("{id:int}", Name = "GetTalk")]
        public async Task<IHttpActionResult> Get(string moniker,int id, bool includeSpeakers = false)
        {
            try
            {
                var result = await _campsRepository.GetTalkByMonikerAsync(moniker,id, includeSpeakers);
                if (result == null) return NotFound();
                //Mapping
                var mapResult = _mapper.Map<TalkModel>(result);
                return Ok(mapResult);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);

            }
        }
        [Route()]
        [HttpPost]
        public async Task<IHttpActionResult> Post(string moniker, TalkModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var camp = await _campsRepository.GetCampAsync(moniker);
                    if (camp != null)
                    {
                        //Mapping
                        var talk = _mapper.Map<Talk>(model);
                        talk.Camp = camp;
                        _campsRepository.AddTalk(talk);
                        if (await _campsRepository.SaveChangesAsync())
                        { 
                            return CreatedAtRoute("GetTalk",new { moniker, id = talk.TalkId},
                                _mapper.Map<TalkModel>(talk));
                        }
                    }                    
                }
                
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);

            }
            return BadRequest(ModelState);
        }
        [Route("{talkId:int}")]
        [HttpPut]
        public async Task<IHttpActionResult> Put(string moniker,int talkId, TalkModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var talk = await _campsRepository.GetTalkByMonikerAsync(moniker, talkId);
                    if (talk == null) return NotFound();
                    
                    //Mapping
                    _mapper.Map(model, talk);
                        
                    if (await _campsRepository.SaveChangesAsync())
                    {
                        return Ok(_mapper.Map<TalkModel>(talk));
                    }                    
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);

            }
            return BadRequest(ModelState);
        }
        [Route("{talkId:int}")]
        [HttpDelete]
        public async Task<IHttpActionResult> Put(string moniker, int talkId)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var talk = await _campsRepository.GetTalkByMonikerAsync(moniker, talkId);
                    if (talk == null) return NotFound();

                    //Mapping
                    _campsRepository.DeleteTalk(talk);
                    if (await _campsRepository.SaveChangesAsync())
                    {
                        return Ok();
                    }
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);

            }
            return BadRequest(ModelState);
        }
    }
}

