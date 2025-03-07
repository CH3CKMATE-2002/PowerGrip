global using System.Text;
global using System.Text.Json;
global using System.Reflection;
global using System.Diagnostics;
global using System.Security.Cryptography;
global using System.Text.Json.Serialization;
global using System.ComponentModel.DataAnnotations;
global using System.ComponentModel.DataAnnotations.Schema;
global using System.IdentityModel.Tokens.Jwt;
global using System.Security.Claims;
global using System.Runtime.InteropServices;
global using System.Net.Mail;

global using Andreas.PowerGrip.Server.Data;
global using Andreas.PowerGrip.Server.Models;
global using Andreas.PowerGrip.Server.Extensions;
global using Andreas.PowerGrip.Server.Controllers;
global using Andreas.PowerGrip.Server.Services.Interfaces;
global using Andreas.PowerGrip.Server.Services;
global using Andreas.PowerGrip.Server.Settings;

global using Andreas.PowerGrip.Shared;
global using Andreas.PowerGrip.Shared.Types;
global using Andreas.PowerGrip.Shared.Types.Auth;
global using Andreas.PowerGrip.Shared.Types.Responses;
global using Andreas.PowerGrip.Shared.Types.Requests;
global using Andreas.PowerGrip.Shared.Utilities;
global using Andreas.PowerGrip.Shared.Extensions;

global using Microsoft.AspNetCore.Mvc;
global using Microsoft.IdentityModel.Tokens;
global using Microsoft.IdentityModel.Protocols.Configuration;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Cryptography.KeyDerivation;
global using Microsoft.AspNetCore.Authentication.JwtBearer;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.Extensions.Options;
// global using Microsoft.Extensions.Logging;  // This is no more after you install Serilog! 

global using Serilog;
