<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DRCOG.Domain.ViewModels.TIP.FundingSourceListViewModel>" %>

<div style='display:none'>
    <div id="dialog-create-funding-source" class="dialog" title="Create New TIP Funding Source">
        <h2 id="dialog-heading">Create a new Funding Source</h2>
        <div class="error" style="display:none;">
          <span></span>.
        </div>
        <fieldset>
            <input type="hidden" id="tipYear" value="<%=Model.TipSummary.TipYear.ToString()%>" />
            <input type="hidden" id="timePeriodId" value="<%=Model.TipSummary.TipYearTimePeriodID.ToString() %>" />
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
                <input type="checkbox" name="FundingSource.IsConformityImpact" id="FundingSource_IsConformityImpact" /> Conformity Impact<br />
            </p>
            <p>
                <span class="big">Funding Type Level</span><br />
                <input type="checkbox" name="FundingSource.IsLocal" id="FundingSource_IsLocal" /> Local<br />
                <input type="checkbox" name="FundingSource.IsState" id="FundingSource_IsState" /> State<br />
                <input type="checkbox" name="FundingSource.IsFederal" id="FundingSource_IsFederal" /> Federal<br />
            </p>
        </fieldset>
    </div>
</div>

<script type="text/javascript">
    var createFundingSourceUrl = '<%=Url.Action("CreateFundingSource","TIP")%>';
    var updateFundingSourceUrl = '<%=Url.Action("UpdateFundingSource","TIP")%>';
    var getFundingSourceUrl = '<%=Url.Action("GetFundingSource","TIP")%>';

    var timeperiod = $("#tipYear").val();
    var timeperiodId = $("#timePeriodId").val();
    var fundingtype = $("#FundingSource_FundingType");
    var code = $("#FundingSource_Code");
    var group = $("#FundingSource_FundingGroup_Id");
    var source = $("#FundingSource_SourceOrganization_OrganizationId");
    var recipient = $("#FundingSource_RecipentOrganization_OrganizationId");
    var discretion = $("#FundingSource_IsDiscreationary");
    var conformity = $("#FundingSource_IsConformityImpact");
    var local = $("#FundingSource_IsLocal");
    var state = $("#FundingSource_IsState");
    var federal = $("#FundingSource_IsFederal");


    $(function () {
        $(document).delegate(".updateFundingSource", "click", function (e) {
            e.preventDefault();

            var $fundingTypeId = $(this).attr("id").replace('source-', '');
            $.colorbox({
                width: "620px",
                height: "525px",
                inline: true,
                href: "#dialog-create-funding-source",
                onLoad: function () {
                    $.ajax({
                        type: "POST",
                        url: getFundingSourceUrl,
                        data: "fundingTypeId=" + $fundingTypeId
                            + "&timePeriodId=" + timeperiodId,
                        dataType: "json",
                        success: function (response) {
                            var data = response.data;
                            //console.log(JSON.stringify(response.data));
                            fundingtype.val(data.FundingType);
                            code.val(data.Code);
                            group.val(data.FundingGroup.Id);
                            source.val(data.SourceOrganization.OrganizationId);
                            recipient.val(data.RecipentOrganization.OrganizationId);
                            discretion.prop("checked", data.IsDiscretionary);
                            conformity.prop("checked", data.IsConformityImpact);
                            local.prop("checked", data.IsLocal);
                            state.prop("checked", data.IsState);
                            federal.prop("checked", data.IsFederal);
                        },
                        error: function (response, error) {
                            //alert('error: ' + error + ' ' + projectName + ' ' + tipYear);
                        }
                    });
                },
                onComplete: function () {
                    var $buttonUpdate = $('<span id="dialog-btn-update" class="cboxBtn">Update</span>').appendTo('#cboxContent');
                    $('#dialog-btn-update').click(function () {
                        //reset form values
                        //$('#projectName').val("");
                        //$('#facilityName').val("");

                        $.ajax({
                            type: "POST",
                            url: updateFundingSourceUrl,
                            data: "FundingTypeId=" + $fundingTypeId
                            + "&FundingGroup.Id=" + group.val()
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
                            success: function (response) {
                                location.reload();
                                console.log("Success update" + $fundingTypeId);
                            },
                            error: function (response, error) {
                                console.log("Error updating" + $fundingTypeId);
                                alert('error: ' + error + ' ' + projectName + ' ' + tipYear);
                            }
                        });

                        return false;
                    });
                },
                onClosed: function () {
                    $("#dialog-btn-update").remove();
                }

            });

        });

        $("#createFundingSource").colorbox({
            width: "620px",
            height: "525px",
            inline: true,
            href: "#dialog-create-funding-source",
            onLoad: function () {

            },
            onComplete: function () {
                var $buttonRegister = $('<span id="dialog-btn-create" class="cboxBtn">Create</span>').appendTo('#cboxContent');
                $('#dialog-btn-create').click(function () {
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
                        success: function (response) {
                            location.reload();
                        },
                        error: function (response, error) {
                            alert('error: ' + error + ' ' + projectName + ' ' + tipYear);
                        }
                    });

                    return false;
                });
            },
            onClosed: function () {
                $("#dialog-btn-create").remove();
            }

        });



    });
    
    

</script>