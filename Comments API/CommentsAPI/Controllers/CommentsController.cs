using CommentsAPI.Models;
using CommentsAPI.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommentsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentsRepository _commentsRepository;
        public CommentsController(ICommentsRepository commentsRepository)
        {
            _commentsRepository = commentsRepository;
        }
        [HttpGet]
        public async Task<IEnumerable<Comments>> GetComments()
        {
            return await _commentsRepository.Get();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Comments>> GetComments(int id)
        {
            return await _commentsRepository.Get(id);
        }
        [HttpPost]
        public async Task<ActionResult<Comments>> PostComments([FromBody] Comments comments)
        {
            var newComments = await _commentsRepository.Create(comments);
            return CreatedAtAction(nameof(GetComments), new { id = newComments.Id }, newComments);
        }
        [HttpPut]
        public async Task<ActionResult> PutComments(int id, [FromBody] Comments comments)
        {
            if(id != comments.Id)
            {
                return BadRequest();
            }
            await _commentsRepository.Update(comments);
            return NoContent();
        }
        [HttpDelete]
        public async Task<ActionResult> Delete (int id)
        {
            var commentsToDelete = await _commentsRepository.Get(id);
            if (commentsToDelete == null)
            {
                return NotFound();
            }
            await _commentsRepository.Delete(commentsToDelete.Id);
            return NoContent();
        }
    }
}
