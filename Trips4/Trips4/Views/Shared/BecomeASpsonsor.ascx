<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DRCOG.Domain.ViewModels.BecomeASponsorViewModel>" %>

<div style='display:none'>
    <div id="dialog-BecomeASponsor" class="dialog" title="Become a Sponsor">
        <h2>Link your account with an existing Agency Contact Sponsor</h2>
        <div id="becomeasponsor-dialog-result" class="" style="display:none;">
          <span></span>.
        </div>
        <form>
        
        <fieldset>
            <p>
                <label for="facilityName">Enter the Sponsor Code:</label>
                <input type="text" name="sponsorcode" class="big" id="sponsorcode" />
            </p>
            <p>
                <label for="sponsorOrganizationId">Confirm the sponsor:</label>
                <%= Html.DropDownListFor(x => x.ProjectSponsorsModel.PrimarySponsor, new SelectList(Model.ProjectSponsorsModel.GetAvailableAgenciesList(), "key", "value", Model.ProjectSponsorsModel.PrimarySponsor.OrganizationId), new { @class = "mediumInputElement big" })%>
                <%--<select class="mediumInputElement big" id="sponsorOrganizationId" name="sponsorOrganizationId" size="1"></select>--%>
            </p>
        </fieldset>
        </form>
    </div>
</div>

<script type="text/javascript">
    var createProjectUrl = "<%=Url.Action("CreateProject","Survey")%>";

    $(function() {
		$("#isSponsorlink").colorbox({ 
            width: "500px", 
            height: "70px", 
            inline: true, 
            href: "#dialog-BecomeASponsor",
            onLoad:function() {
            },
            onComplete: function () {
                $.fn.colorbox.resize();
                var buttonCompletedSave = $('<span id="btn-BecomeASponsor" class="cboxBtn">Submit</span>').appendTo('#cboxContent');
                setTimeout(function() { $("#sponsorcode").focus(); }, 1000);
                
                $('#btn-BecomeASponsor').click(function() {
                    var sponsorCode = $("#sponsorcode").val(),
			            sponsorOrganizationId = $("#ProjectSponsorsModel_PrimarySponsor option:selected").val();
            		
		            if (sponsorCode === '' || sponsorOrganizationId === '') {
		                $("div#becomeasponsor-dialog-result span").html("All fields are required");
                        $("div#becomeasponsor-dialog-result").addClass("error").autoHide({ wait: 10000, removeClass: "error", resizeColorbox: true });
                        $.fn.colorbox.resize();
		            } else {
                        //reset form values
		                $('#sponsorcode').val("");
                		
		                $.ajax({
                            type: "POST",
                            url: createProjectUrl,
                            data: "projectName=" + projectName 
                                + "&sponsorOrganizationId=" + sponsorOrganizationId,
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
            onCleanup: function() { $("#btn-BecomeASponsor").remove(); $("#btn-BecomeASponsor").unbind(); }
        });
    });
</script>