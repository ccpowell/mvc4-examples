<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DRCOG.Domain.ViewModels.Survey.CreateProjectViewModel>" %>

<div style='display:none'>
    <div id="dialog-create-project" class="dialog"> <%--title="Create New <%if (Model.Survey.AgencyProjectList != null) { %><%= Model.Survey.AgencyProjectList.FirstOrDefault().SponsorName.ToString()%><% } %> Project"--%>
        <%--<h2>Create a new <%if (Model.Survey.AgencyProjectList != null) { %> <%= Model.Survey.AgencyProjectList.FirstOrDefault().SponsorName.ToString()%><% } %> Project</h2>--%>
        <div id="create-dialog-result" class="" style="display:none;">
          <span></span>.
        </div>
        <form>
        
        <fieldset>
            <input type="hidden" id="timePeriodId" value="<%= Model.Survey.Id.ToString() %>" />
            <input type="hidden" id="timePeriod" value="<%= Model.Survey.Name.ToString() %>" />
            
            
            <p>
                <label for="facilityName">Facility Name:</label>
                <input type="text" name="facilityName" class="big" id="facilityName" />
            </p>
            <%--<p>
                <label for="projectName">Project Location:</label>
                <input type="text" name="projectName" class="big" id="projectName" />
            </p>--%>
            <p>
                <%= Html.LabelFor(x => x.Project.BeginAt) %>
                <%= Html.TextBoxFor(x => x.Project.BeginAt, new { @class = "mediumInputElement big" } ) %>
            </p>
            <p>
                <%= Html.LabelFor(x => x.Project.EndAt) %>
                <%= Html.TextBoxFor(x => x.Project.EndAt, new { @class = "mediumInputElement big" })%>
            </p>
            <p>
                <label>Improvement Type:</label>
                <%= Html.DropDownListFor(x => x.Project.ImprovementTypeId
                    , new SelectList(Model.AvailableImprovementTypes, "key", "value")
                    , new { @class = "mediumInputElement big" })%>
            </p>
            <% if (Model.Survey.SponsorsOrganization != null && !Model.Survey.SponsorsOrganization.OrganizationId.Equals(default(int)))
               { %>
            <input type="hidden" id="sponsorOrganizationId" value="<%= Model.Survey.SponsorsOrganization.OrganizationId %>" /> <%--(int)ViewData["PersonOrganizationId"]--%>
            <% } else { %>
            <p>
                <label for="sponsorOrganizationId">Sponsor:</label>
                <%= Html.DropDownListFor(x => x.Survey.ProjectSponsorsModel.PrimarySponsor, new SelectList(Model.Survey.ProjectSponsorsModel.GetAvailableAgenciesList(), "key", "value", Model.Survey.ProjectSponsorsModel.PrimarySponsor.OrganizationId), new { @class = "mediumInputElement big" })%>
                <%--<select class="mediumInputElement big" id="sponsorOrganizationId" name="sponsorOrganizationId" size="1"></select>--%>
            </p>
            <% } %>
            <%--<p>
                <label for="AmendmentTypes">Amendment Type:</label>
                <%= Html.DropDownListFor(x => x.AmendmentTypes, new SelectList(Model.AmendmentTypes, "Key", "Value"), new { @class = "mediumInputElement big" })%>
            </p>--%>
            <%--<button type="submit" id="add-project" class="fg-button big ui-state-default ui-priority-primarystate-enabled ui-corner-all" >Create</button>--%>
        </fieldset>
        </form>
        
    </div>
</div>
<script type="text/javascript">
    var createProjectUrl = "<%=Url.Action("CreateProject","Survey")%>";

    $(function() {
		$("#button-create-project").colorbox({ 
            width: "540px", 
            height: "70px", 
            inline: true, 
            href: "#dialog-create-project",
            onLoad:function() {
            },
            onComplete: function () {
                $.fn.colorbox.resize();
                var buttonCompletedSave = $('<span id="btn-add-project" class="cboxBtn">Create New Project</span>').appendTo('#cboxContent');
                setTimeout(function() { $("#facilityName").focus(); }, 1000);
                
                $('#btn-add-project').click(function() {
                    var startAt = $("#Project_BeginAt").val(),
                        endAt = $("#Project_EndAt").val(),
                        improvementTypeSelected = $("#Project_ImprovementTypeId option:selected"),
                        facilityName = $("#facilityName").val(),
			            projectName = startAt + ' to ' + endAt + ' ' + improvementTypeSelected.text(),
                        timePeriodId = $("#timePeriodId").val(),
			            timePeriod = $("#timePeriod").val(),
			            <% if(Model.Survey.SponsorsOrganization != null && !Model.Survey.SponsorsOrganization.OrganizationId.Equals(default(int))) { %>
			            sponsorContactId = <%= (int)ViewData["PersonId"]%>,
			            sponsorOrganizationId = $("#sponsorOrganizationId").val();
			            <% } else { %>
			            sponsorContactId = 0
			            sponsorOrganizationId = $("#Survey_ProjectSponsorsModel_PrimarySponsor option:selected").val();
			            <% } %>
                        //alert(sponsorOrganizationId); //Debug only
		            if (projectName === '' || facilityName === '' || sponsorOrganizationId === '' || timePeriodId === '') {
		                $("div#create-dialog-result span").html("All fields are required");
                        $("div#create-dialog-result").addClass("error").autoHide({ wait: 10000, removeClass: "error", resizeColorbox: true });
                        $.fn.colorbox.resize();
		            } else {
                        //reset form values
		                //$('#projectName').val("");
		                //$('#facilityName').val("");
                		
		                $.ajax({
                            type: "POST",
                            url: createProjectUrl,
                            data: "projectName=" + projectName 
                                + "&facilityName=" + facilityName 
                                + "&timePeriodId=" + timePeriodId
                                + "&sponsorContactId=" + sponsorContactId
                                + "&sponsorOrganizationId=" + sponsorOrganizationId
                                + "&improvementTypeId=" + improvementTypeSelected.val()
                                + "&startAt=" + startAt
                                + "&endAt=" + endAt,
                            dataType: "json",
                            success: function(response, textStatus, XMLHttpRequest) {
                                if (response.error == "false") {
                                    //Add into the DOM
                                    var projectversionid = response.data;
                                    //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! need top change the action link
                                    var redirectActionUrl = '<%= Url.Action("Info","Survey", new { @message = "r_message", @year = "r_timeperiod", @id = "r_projectversionid"  }) %>'
                                    redirectActionUrl = redirectActionUrl.replace("r_timeperiod", timePeriod);
                                    redirectActionUrl = redirectActionUrl.replace("r_projectversionid", projectversionid);
                                    redirectActionUrl = redirectActionUrl.replace("r_message", response.message);
                                    
                                    //"/trips/Project/" + tipYear + "/Info/" + projectversionid + "?message=Project created successfully.";
                                    location = redirectActionUrl;
                                } else {
                                    alert("error: " + response.message);
                                }
                            },
                            error: function(response, textStatus, AjaxException) {
                                alert('error: '+AjaxException+' '+projectName+' '+timePeriodId);
                            }
                        });
		            }
            		
                    return false;
                });
                
            },
            onCleanup: function() { $("#btn-add-project").remove(); $("#btn-add-project").unbind(); }
        });
        
    });
    
    

</script>