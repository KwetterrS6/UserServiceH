using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using UserService.AsyncDataServices;
using UserService.Data;
using UserService.Dtos;
using UserService.Models;
using UserService.SyncDataServices.Http;

namespace UserService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepo _repository;
        private readonly IMapper _mapper;
        private readonly IKweetDataClient _kweetDataClient;
        private readonly IMessageBusClient _messageBusClient;

        public UsersController(
            IUserRepo repository, 
            IMapper mapper,
            IKweetDataClient kweetDataClient,
            IMessageBusClient messageBusClient)
        {
            _repository = repository;
            _mapper = mapper;
            _kweetDataClient = kweetDataClient;
            _messageBusClient = messageBusClient;
        }

        [HttpGet]
        public ActionResult<IEnumerable<UserReadDto>> GetUsers()
        {
            Console.WriteLine("--> Getting Users....");

            var Users = _repository.GetAllUsers();

            return Ok(_mapper.Map<IEnumerable<UserReadDto>>(Users));
        }

        [HttpGet("{id}", Name = "GetUserById")]
        public ActionResult<UserReadDto> GetUserById(int id)
        {
            var user = _repository.GetUserById(id);
            if (user != null)
            {
                return Ok(_mapper.Map<UserReadDto>(user));
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<ActionResult<UserReadDto>> Register(UserCreateDto userCreateDto)
        {
            var userModel = _mapper.Map<User>(userCreateDto);
            if(_repository.CheckUserExist(userModel.Email) == false)
            {
                _repository.Register(userModel);
                _repository.SaveChanges();
            }
            else
            {
                Console.WriteLine("user already exists");
            }
           
            var UserReadDto = _mapper.Map<UserReadDto>(userModel);
            // Send Sync Message
            try
            {
                await _kweetDataClient.SendUserToKweet(UserReadDto);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"--> Could not sendsdfsdfsdf synchronously: {ex.Message}");
            }

            //Send Async Message
            try
            {
                var userCreatedDto = _mapper.Map<UserCreatedDto>(UserReadDto);
                _messageBusClient.CreateNewUser(userCreatedDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could notdsdfdfsd send asynchronously: {ex.Message}");
            }

            return CreatedAtRoute(nameof(GetUserById), new { Id = UserReadDto.Id}, UserReadDto);
        }
/*
        [HttpGet("/login")]
            public async Task<ActionResult<UserReadDto>> LoginAsync(String email, String password)
            {
                _repository.SaveChanges();

                var userReadDto = _mapper.Map<UserReadDto>(_repository.Login(email, password));

                try
                {
                    await _kweetDataClient.SendUserToKweet(userReadDto);
                }
                catch (Exception ex)
                {

                    Console.WriteLine($"could nottttttt send synchronously: {ex.Message}");
                }

                return userReadDto;
            }*/
    }
}