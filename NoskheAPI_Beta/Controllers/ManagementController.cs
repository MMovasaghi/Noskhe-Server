using Microsoft.AspNetCore.Mvc;
using NoskheAPI_Beta.CustomExceptions.Management;
using NoskheAPI_Beta.Models;
using NoskheAPI_Beta.Models.Response;
using NoskheAPI_Beta.Services;
using NoskheAPI_Beta.Settings.ResponseMessages.Management;
using NoskheAPI_Beta.Settings.Routing.Management;

namespace NoskheAPI_Beta.Controllers
{
    [Route("desktop-api/[controller]")]
    [ApiController]
    public class ManagementController : ControllerBase
    {
        private IManagementService _managementService;
        public ManagementController(IManagementService managementService)
        {
            _managementService = managementService;
        }

        // POST: desktop-api/management/new-medicine
        [HttpPost(Labels.AddNewMedicine)]
        public ActionResult AddNewMedicine([FromBody] Medicine newMedcine)
        {
            try
            {
                return Ok(_managementService.AddNewMedicine(newMedcine));
            }
            catch(DuplicateMedicineException dme)
            {
                return BadRequest(new ResponseTemplate {
                    Success = false,
                    Error = dme.Message
                });
            }
            catch
            {
                return BadRequest(new ResponseTemplate {
                    Success = false,
                    Error = ErrorCodes.APIUnhandledExceptionMsg
                });
            }
        }

        // POST: desktop-api/management/edit-medicine
        [HttpPost(Labels.EditMedicine)]
        public ActionResult EditMedicine([FromBody] Medicine existingMedcine)
        {
            try
            {
                return Ok(_managementService.EditMedicine(existingMedcine));
            }
            catch(NoMedicinesAvailableException nmae)
            {
                return BadRequest(new ResponseTemplate {
                    Success = false,
                    Error = nmae.Message
                });
            }
            catch
            {
                return BadRequest(new ResponseTemplate {
                    Success = false,
                    Error = ErrorCodes.APIUnhandledExceptionMsg
                });
            }
        }

        // GET: desktop-api/management/all-medicines
        [HttpGet(Labels.ShowAllMedicines)]
        public ActionResult ShowAllMedicines()
        {
            try
            {
                return Ok(_managementService.ShowAllMedicines());
            }
            catch
            {
                return BadRequest(new ResponseTemplate {
                    Success = false,
                    Error = ErrorCodes.APIUnhandledExceptionMsg
                });
            }
        }
    }
}