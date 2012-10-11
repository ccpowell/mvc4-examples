<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DRCOG.Domain.ViewModels.TIP.FundingSourceListViewModel>" %>

<div style='display:none'>
    <div id="dialog-create-funding-source" class="dialog" title="Create New TIP Funding Source">
        <h2>Create a new Funding Source</h2>
        <div class="error" style="display:none;">
          <span></span>.
        </div>
        <form>
        <fieldset>
            <input type="hidden" id="tipYear" value="<%=Model.TipSummary.TipYear.ToString()%>" />
            <p>
                <%= Html.LabelFor(x => x.FundingSource.FundingType) %>
                <%= Html.TextBoxFor(x => x.FundingSource.FundingType, new { @class = "big", @maxlength = 100 })%>
            </p>
            <p>
                <%= Html.LabelFor(x => x.FundingSource.Code) %>
                <%= Html.TextBoxFor(x => x.FundingSource.Code, new { @class = "big", @maxlength = 50 })%>
            </p>
            <p>
                <label for="FundingSource_FundingGroup_Id">Funding Group:</label>
                <%= Html.DropDownListFor(x => x.FundingSource.FundingGroup.Id, new SelectList(Model.FundingGroups, "Key", "Value"), new { @class = "mediumInputElement big" })%>
            </p>
            <p>
                <label for="FundingSource_SourceOrganization_OrganizationId">Source:</label>
                <%= Html.DropDownListFor(x => x.FundingSource.SourceOrganization.OrganizationId, new SelectList(Model.SourceAgencies, "Key", "Value"), new { @class = "mediumInputElement big" })%>
            </p>
            <p>
                <label for="FundingSource_RecipentOrganization_OrganizationId">Recipient:</label>
                <%= Html.DropDownListFor(x => x.FundingSource.RecipentOrganization.OrganizationId, new SelectList(Model.RecipientAgencies, "Key", "Value"), new { @class = "mediumInputElement big" })%>
            </p>
            <p>
                <input type="checkbox" name="FundingSource.IsDiscretionary" id="FundingSource_IsDiscreationary" /> Discretion<br />
                <input type="checkbox" name="FundingSource.IsConformityImpact" id="FundingSource_IsConformityImpact" /> Conformity Impact
            </p>
            <p>
                <span class="big">Funding Type Level</span><br />
                <input type="checkbox" name="FundingSource.IsLocal" id="FundingSource_IsLocal" /> Local<br />
                <input type="checkbox" name="FundingSource.IsState" id="FundingSource_IsState" /> State<br />
                <input type="checkbox" name="FundingSource.IsFederal" id="FundingSource_IsFederal" /> Federal<br />
            </p>
        </fieldset>
        </form>
    </div>
</div>

<script type="text/javascript">
    var createFundingSourceUrl = "<%=Url.Action("CreateFundingSource","TIP")%>";
    $(function() {
		$("#createFundingSource").colorbox({ 
            width: "620px", 
            height: "525px", 
            inline: true, 
            href: "#dialog-create-funding-source",
            onLoad:function() {
            
            },
            onComplete: function() {
                var $buttonRegister = $('<span id="dialog-btn-create" class="cboxBtn">Create</span>').appendTo('#cboxContent');
                $('#dialog-btn-create').click(function() {
                    var timeperiod = $("#tipYear").val();
                    var fundingtype = $("#FundingSource_FundingType");
                    var code = $("#FundingSource_Code");
                    var group = $("#FundingSource_FundingGroup_Id");
                    var source = $("#FundingSource_SourceOrganization_OrganizationId");
                    var recipient = $("#FundingSource_RecipentOrganization_OrganizationId");
                    var discretion = $("#FundingSource_IsDiscreationary");
                    var conformity = $("#FundingSource_IsConformityImpact");
                    var local = $("FundingSource_IsLocal");
                    var state = $("FundingSource_IsState");
                    var federal = $("FundingSource_IsFederal");
                    
		            //reset form values
		            //$('#projectName').val("");
		            //$('#facilityName').val("");
            		
		            $.ajax({
                        type: "POST",
                        url: createFundingSourceUrl,
                        data: "FundingGroup.Id=" + group.val()
                            + "&SourceOrganization.OrganizationId=" + source.val()
                            + "&RecipentOrganization.OrganizationId=" + recipient.val()
                            + "&TimePeriod=" + timeperiod
                            + "&FundingType=" + fundingtype.val()
                            + "&Code=" + code.val()
                            + "&IsDiscretionary=" + discretion.is(':checked')
                            + "&IsConformityImpact=" + conformity.is(':checked')
                            + "&IsLocal=" + local.is(':checked')
                            + "&IsState=" + state.is(':checked')
                            + "&IsFederal=" + federal.is(':checked'),
                        dataType: "json",
                        success: function(response) {
                            location.reload();
                            
                            //Add into the DOM
                            //var projectversionid = response;
                            //var redirectActionUrl = "<%= Url.Action("Info","Project", new { @message = "Project created successfully.", @year = "timeperiod", @id = "projectversionid"  }) %>"
                            //redirectActionUrl = redirectActionUrl.replace("timeperiod", timeperiod);
                            
                            //"/Project/" + tipYear + "/Info/" + projectversionid + "?message=Project created successfully.";
                            //location = redirectActionUrl;
                            //autoHide();
                        },
                        error: function(response, error) {
                            alert('error: '+error+' '+projectName+' '+tipYear);
                        }
                    });
                    
                    return false;
                });
            },
            onClosed: function() {
                $("#dialog-btn-create").remove();
            }
            
        });
        
    });
    
    

</script>