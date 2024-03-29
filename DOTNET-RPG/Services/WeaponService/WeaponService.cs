﻿using AutoMapper;
using DOTNET_RPG.Data;
using DOTNET_RPG.Dtos.Character;
using DOTNET_RPG.Dtos.Weapon;
using DOTNET_RPG.Models;
using DOTNET_RPG.Models.Responses;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DOTNET_RPG.Services.WeaponService
{
    public class WeaponService : IWeaponService
    {
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public WeaponService(DataContext context, IHttpContextAccessor _httpContext, IMapper mapper)
        {
            _httpContextAccessor = _httpContext;
            _mapper = mapper;
            _context = context;
        }

        public async Task<ServiceResponse<GetCharacterDto>> AddWeapon(AddWeaponDto newWeapon)
        {
            var response = new ServiceResponse<GetCharacterDto>();
            try
            {
                var character = await _context.Characters
                    .FirstOrDefaultAsync(c => c.Id == newWeapon.CharacterId &&
                        c.User!.Id == int.Parse(_httpContextAccessor.HttpContext!.User
                        .FindFirstValue(ClaimTypes.NameIdentifier)!));

                if (character is null)
                {
                    response.Success = false;
                    response.Message = "Character not found.";
                    return response;
                }

                var weapon = new Weapon
                {
                   Name = newWeapon.Name,
                   Damage = newWeapon.Damage,
                   Character = character
                };

                _context.Weapons.Add(weapon);
                _context.SaveChanges();

                response.Data = _mapper.Map<GetCharacterDto>(character);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
