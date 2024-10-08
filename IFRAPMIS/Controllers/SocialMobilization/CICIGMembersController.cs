﻿using BAL.IRepository.BeneficiaryVerification;
using BAL.IRepository.SocialMobilization;
using DAL.Models.Domain.MasterSetup;
using DAL.Models.Domain.SocialMobilization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace IFRAPMIS.Controllers.SocialMobilization
{
    public class CICIGMembersController : Controller
    {
            private readonly ICICIGMember _context;
            private readonly IBeneficiaryVerified _member;

            public CICIGMembersController(ICICIGMember context, IBeneficiaryVerified member)
            {
                _context = context;
                _member = member;
            }

            // GET: CommunityInstituteMembers
            public async Task<IActionResult> Index()
            {
                return View(await _context.GetAll());
            }
            public async Task<IActionResult> _Index(int id)
            {
                ViewBag.CommunityInstitutionId = id;
                return PartialView(await _context.GetAllCIMemberById(id));
            }
            // GET: CommunityInstituteMembers/Details/5
            public async Task<IActionResult> Details(int? id)
            {
                if (id == null || _context == null)
                {
                    return NotFound();
                }

                var communityInstituteMember = await _context.GetFromBVById(id);
                if (communityInstituteMember == null)
                {
                    return NotFound();
                }

                return View(communityInstituteMember);
            }
            [HttpPost]
            public async Task<JsonResult> AjaxMemberInformation(string id)
            {
                var Info = await _context.GetMemberByCNIC(id);
                if (Info == null)
                {
                    return Json(new { isValid = false });
                }
                return Json(new { isValid = true, Info });
            }
            // GET: CommunityInstituteMembers/Create
            public async Task<IActionResult> Create(int id)
            {
                ViewBag.CIId = id;
                BeneficiaryVerified obj = new BeneficiaryVerified();
                ViewBag.CommunityInstitutionId = id;
                //obj.BeneficiaryTypeId = 1;//KDA
                var commInst = _context.GetCI(id);
                ViewBag.IsMaleGender = commInst.Result.Gender == "Male" ? true : false;
                ViewBag.IsFemaleGender = commInst.Result.Gender == "Female" ? true : false;
                ViewData["CommunityInstitutionId"] = new SelectList(_context.GetAllCI(), "CommunityInstitutionId", "Name", id);


                var designations = new[]
                {
                    new { DesignationId = 1, DesignationName = "Manager" },
                    new { DesignationId = 2, DesignationName = "Engineer" },
                    new { DesignationId = 3, DesignationName = "Technician" }
                };

                // Create the SelectList and pass it to ViewData
                ViewData["DesignationId"] = new SelectList(designations, "DesignationId", "DesignationName", "BeneficiaryVerified");

                //ViewData["DesignationId"] = new SelectList(await _context.GetAllDesignation(), "DesignationId", "DesignationName", "BeneficiaryVerified");
                return View(obj);
            }

            // POST: CommunityInstituteMembers/Create
            // To protect from overposting attacks, enable the specific properties you want to bind to.
            // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Create(BeneficiaryVerified member, IFormFile ProfilePicture, int CIId, int MemberVillageId, int MemberId, string DesignationName)
            {
                if (ModelState.IsValid)
                {
                    if (member.CNIC.IndexOf('_') > 0)
                    {
                        ModelState.AddModelError(nameof(member.CNIC), "Invalid CNIC!");
                        ViewData["CommunityInstitutionId"] = new SelectList(_context.GetAllCI(), "CommunityInstitutionId", "Name", CIId);
                        var designation = new[]
                        {
                            new { DesignationId = 1, DesignationName = "Manager" },
                            new { DesignationId = 2, DesignationName = "Engineer" },
                            new { DesignationId = 3, DesignationName = "Technician" }
                        };                    
                        // Create the SelectList and pass it to ViewData
                        ViewData["DesignationId"] = new SelectList(designation, "DesignationId", "DesignationName", "BeneficiaryVerified");

                        //ViewData["DesignationId"] = new SelectList(await _context.GetAllDesignation(), "DesignationId", "DesignationName", DesignationId);
                        return View(member);
                    }
                    var CI = await _context.GetCI(CIId);
                    if (false)//MemberId == 0)
                    {
                        //member.VillageId = CI.VillageId;
                        _member.Insert(member, ProfilePicture);
                        MemberId = _member.Max();
                    }
                    else
                    {
                        var output = _context.IsMemberExist(member.CNIC);
                        if (output != 0)
                        {
                            MemberId = output;
                        }
                        else
                        {
                            //member.VillageId = CI.VillageId;
                            _member.Insert(member, ProfilePicture);
                            MemberId = _member.Max();
                        }

                        var result = _context.CountMemberInCIM(MemberId);
                        if (result > 0)
                        {
                            ModelState.AddModelError(nameof(member.CNIC), "Already member added with same CNIC!");
                            return BadRequest(ModelState);
                        }
                        /* result = _context.CountMemberWithCellInCIM(member.CellNo, CIId);
                         if (result > 0)
                         {
                             ModelState.AddModelError(nameof(member.CellNo), "Already member added with same Cell number!");
                             return BadRequest(ModelState);
                         }*/
                        /*if(CI.VillageId != MemberVillageId)
                        {
                            var IsInLIP = _context.CountMemberInLIP(MemberId);
                            if(IsInLIP> 0)
                            {
                                ModelState.AddModelError(nameof(member.CellNo), "Already member exist in LIP with different village!");
                                return BadRequest(ModelState);
                            }                        
                        }*/
                    }
                    _context.Insert(MemberId, CIId, DesignationName);
                    return RedirectToAction(nameof(Create), new { CIId });
                }
                ViewData["CommunityInstitutionId"] = new SelectList(_context.GetAllCI(), "CommunityInstitutionId", "Name", CIId);

                var designations = new[]
                   {
                        new { DesignationId = 1, DesignationName = "Manager" },
                        new { DesignationId = 2, DesignationName = "Engineer" },
                        new { DesignationId = 3, DesignationName = "Technician" }
                    };
                // Create the SelectList and pass it to ViewData
                ViewData["DesignationId"] = new SelectList(designations, "DesignationId", "DesignationName", "BeneficiaryVerified");

                //ViewData["DesignationId"] = new SelectList(await _context.GetAllDesignation(), "DesignationId", "DesignationName", DesignationId);
                return View(member);
            }

            public async Task<IActionResult> AddBeneficiary(string url, string message)
            {
                ViewBag.url = url;
                ViewBag.message = message;
                BeneficiaryVerified obj = new BeneficiaryVerified();
                //obj.BeneficiaryTypeId = 2;//KDA
                ViewData["DistrictId"] = new SelectList(await _context.GetDistricts(User), "DistrictId", "Name");
                return View(obj);
            }
            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> AddBeneficiary(BeneficiaryVerified member, IFormFile ProfilePicture, string url)
            {
                if (ModelState.IsValid)
                {
                    var result = _member.Count(member.CNIC);
                    if (result > 0)
                    {
                        ModelState.AddModelError(nameof(member.CNIC), "CNIC already exist!");
                        return BadRequest(ModelState);
                    }
                    //result = _member.CountCell(member.CellNo);
                    //if (result > 0)
                    //{
                    //    ModelState.AddModelError(nameof(member.CNIC), "Mobile number already exist!");
                    //    return BadRequest(ModelState);
                    //}
                    if (ProfilePicture != null && ProfilePicture.Length > 0)
                    {
                        await using var memoryStream = new MemoryStream();
                        await ProfilePicture.CopyToAsync(memoryStream);
                        member.ProfilePicture = memoryStream.ToArray();
                    }
                    await _member.Insert(member, ProfilePicture);
                    await _member.Save();
                    return RedirectToAction(nameof(AddBeneficiary), new { url = url });
                }
                return View(member);
            }
            // GET: CommunityInstituteMembers/Edit/5
            
        
        public async Task<IActionResult> Edit(int? id, int? CIMId, int CIId)
            {
                if (id == null || CIMId == null)
                {
                    return NotFound();
                }
                ViewBag.MemberId = id;
                ViewBag.CIId = CIId;
                ViewBag.CIMId = CIMId;
                var member = await _member.GetById(id);
                if (member == null)
                {
                    return NotFound();
                }
                var listCI = _context.GetAllCI();
                ViewData["CommunityInstitutionId"] = new SelectList(listCI.Where(a => a.CICIGId == CIId), "CICIGId", "Name");
    
                var designations = new[]
                      {
                            new { DesignationId = 1, DesignationName = "Manager" },
                            new { DesignationId = 2, DesignationName = "Engineer" },
                            new { DesignationId = 3, DesignationName = "Technician" }
                        };
                // Create the SelectList and pass it to ViewData
                ViewData["DesignationId"] = new SelectList(designations, "DesignationId", "DesignationName", "BeneficiaryVerified");

                //ViewData["DesignationId"] = new SelectList(await _context.GetAllDesignation(), "DesignationId", "DesignationName");
                return View(member);
            }

            // POST: CommunityInstituteMembers/Edit/5
            // To protect from overposting attacks, enable the specific properties you want to bind to.
            // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Edit(int id, int CIId, int CIMId, BeneficiaryVerified member, IFormFile ProfilePicture, string DesignationName)
            {
                if (id != member.BeneficiaryVerifiedId)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        _member.Update(member, ProfilePicture);
                        CIMember communityInstituteMember = await _context.GetFromBVById(CIMId);
                        if (communityInstituteMember.Designation != DesignationName)
                        {
                            communityInstituteMember.Designation = DesignationName;
                            _context.Update(communityInstituteMember);
                        }
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!CommunityInstituteMemberExists(member.BeneficiaryVerifiedId))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    return RedirectToAction("Details", "CommunityInstitutions", new { id = CIId });
                }
                ViewData["CommunityInstitutionId"] = new SelectList(_context.GetAllCI(), "CommunityInstitutionId", "Name", CIId);

                var designations = new[]
                          {
                                new { DesignationId = 1, DesignationName = "Manager" },
                                new { DesignationId = 2, DesignationName = "Engineer" },
                                new { DesignationId = 3, DesignationName = "Technician" }
                            };
                // Create the SelectList and pass it to ViewData
                ViewData["DesignationId"] = new SelectList(designations, "DesignationId", "DesignationName", "BeneficiaryVerified");

                //ViewData["DesignationId"] = new SelectList(await _context.GetAllDesignation(), "DesignationId", "DesignationName", DesignationId);
                return View(member);
            }

            // GET: CommunityInstituteMembers/Delete/5
            public async Task<IActionResult> Delete(int? id)
            {
                if (id == null || _context == null)
                {
                    return NotFound();
                }

                var communityInstituteMember = await _context.GetById(id);
                if (communityInstituteMember == null)
                {
                    return NotFound();
                }

                return View(communityInstituteMember);
            }

            // POST: CommunityInstituteMembers/Delete/5
            [HttpPost, ActionName("Delete")]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> DeleteConfirmed(int id)
            {
                if (_context == null)
                {
                    return Problem("Entity set 'ApplicationDbContext.CommunityInstituteMember'  is null.");
                }
                var communityInstituteMember = await _context.GetById(id);
                if (communityInstituteMember != null)
                {
                    _context.Remove(communityInstituteMember);
                }

                return RedirectToAction("Details", "CICIG", new { id = communityInstituteMember.CICIGId });
            }

            private bool CommunityInstituteMemberExists(int id)
            {
                return _context.Exist(id);
            }
        }
    }
