
import { useState, useEffect, forwardRef } from "react";
import TextField from "@mui/material/TextField";
import { Container, Typography } from "@mui/material";
import Grid from "@mui/material/Grid";
import { styled } from "@mui/material/styles";
import Box from "@mui/material/Box";
import Paper from "@mui/material/Paper";

import Dialog from '@mui/material/Dialog';
import Fade from '@mui/material/Fade';
import Button from '@mui/material/Button';
import AttachFileRoundedIcon from '@mui/icons-material/AttachFileRounded';
import ShareRoundedIcon from '@mui/icons-material/ShareRounded';
import TextsmsRoundedIcon from '@mui/icons-material/TextsmsRounded';
import IconButton from '@mui/material/IconButton';
import Card from '@mui/material/Card';
import NoteHeader from "./NoteHeader";
import MoreDropdown from "./MoreDropdown";
import NoteShare from "./NoteShareModal";

// ** Third Party Components
import axios from 'axios';

// import Typography from "@mui/material";

const Item = styled(Paper)(({ theme }) => ({
  backgroundColor: theme.palette.mode === "dark" ? "#1A2027" : "#fff",
  ...theme.typography.body2,
  padding: theme.spacing(1),
  textAlign: "center",
  color: theme.palette.text.secondary,
}));

const Transition = forwardRef(function Transition(props, ref) {
  return <Fade ref={ref} {...props} />
})

const Note = ({ apiData, closeModal }) => {

  const baseURL = "https://webapidev.aitalkx.com/nts/query/GetNoteDetails?templateCode=HRLocation&userId=45bba746-3309-49b7-9c03-b5793369d73c";
  //const manageURL = "https://webapidev.aitalkx.com/nts/command/ManageNote";
  const manageURL = "https://webapidev.aitalkx.com/nts/command/ManageNote";

  //Note Page
  const [Id, setId] = useState([])
  const [TemplateCode, setTemplateCode] = useState([])
  const [TemplateId, setTemplateId] = useState([])
  const [NoteTemplateId, setNoteTemplateId] = useState([])
  const [TableMetadataId, setTableMetadataId] = useState([])
  const [NoteNo, setNoteNo] = useState([])
  const [NoteNoText, setNoteNoText] = useState([])
  const [NumberGenerationType, setNumberGenerationType] = useState([])
  const [IsInEditMode, setIsInEditMode] = useState([])
  const [HideHeader, setHideHeader] = useState([])
  const [HideSubject, setHideSubject] = useState([])
  const [SubjectText, setSubjectText] = useState([])
  const [NoteSubject, setNoteSubject] = useState([])
  const [HideDescription, setHideDescription] = useState([])
  const [DescriptionText, setDescriptionText] = useState([])
  const [NoteDescription, setNoteDescription] = useState([])
  const [EnableSequenceOrder, setEnableSequenceOrder] = useState([])
  const [SequenceOrder, setSequenceOrder] = useState([])
  const [HideOwner, setHideOwner] = useState([])
  const [HidePriority, setHidePriority] = useState([])
  const [NotePriorityId, setNotePriorityId] = useState([])
  const [HideStartDate, setHideStartDate] = useState([])
  const [HideExpiryDate, setHideExpiryDate] = useState([])
  const [HideReminderDate, setHideReminderDate] = useState([])
  const [StartDate, setStartDate] = useState([])
  const [ExpiryDate, setExpiryDate] = useState([])
  const [ReminderDate, setReminderDate] = useState([])
  const [RequestedByUserId, setRequestedByUserId] = useState([])
  const [OwnerUserId, setOwnerUserId] = useState([])
  const [OwnerUserName, setOwnerUserName] = useState([])
  const [IsSubmitButtonVisible, setIsSubmitButtonVisible] = useState([])
  const [IsVersioningButtonVisible, setIsVersioningButtonVisible] = useState([])
  const [IsExpireButtonVisible, setIsExpireButtonVisible] = useState([])
  const [CompleteButtonText, setCompleteButtonText] = useState([])
  const [NoteId, setNoteId] = useState([])
  const [ParentNoteId, setParentNoteId] = useState([])
  const [ParentTaskId, setParentTaskId] = useState([])
  const [ParentServiceId, setParentServiceId] = useState([])
  const [IsDraftButtonVisible, setIsDraftButtonVisible] = useState([])
  const [SaveAsDraftText, setSaveAsDraftText] = useState([])
  const [NoteStatusName, setNoteStatusName] = useState([])
  const [VersionNo, setVersionNo] = useState([])
  const [CreatedBy, setCreatedBy] = useState([])
  const [LastUpdatedBy, setLastUpdatedBy] = useState([])
  const [LegalEntityId, setLegalEntityId] = useState([])
  const [CompanyId, setCompanyId] = useState([])
  const [ReferenceType, setReferenceType] = useState([])
  const [ReferenceId, setReferenceId] = useState([])
  const [ServicePlusId, setServicePlusId] = useState([])
  const [NotePlusId, setNotePlusId] = useState([])
  const [TaskPlusId, setTaskPlusId] = useState([])
  const [NoteStatusCode, setNoteStatusCode] = useState([])
  const [PortalId, setPortalId] = useState([])


  const [showShare, setShowShare] = useState(false)


  const viewData = async () => {


    let colresponse = await axios.get(baseURL)

    const model = colresponse.data;
    console.log(model, "model");

    setId(model.Id);
    setTemplateCode(model.TemplateCode);
    setTemplateId(model.TemplateId);
    setTableMetadataId(model.TableMetadataId);
    setNoteTemplateId(model.NoteTemplateId);
    setNoteNo(model.NoteNo);
    setNoteNoText(model.NoteNoText);
    setNumberGenerationType(model.NumberGenerationType);
    setIsInEditMode(model.IsInEditMode);
    setHideHeader(model.HideHeader);
    setHideSubject(model.HideSubject);
    setSubjectText(model.SubjectText);
    setNoteSubject(model.NoteSubject);
    setHideDescription(model.HideDescription);
    setDescriptionText(model.DescriptionText);
    setNoteDescription(model.NoteDescription);
    setEnableSequenceOrder(model.EnableSequenceOrder);
    setSequenceOrder(model.SequenceOrder);
    setHideOwner(model.HideOwner);
    setHidePriority(model.HidePriority);
    setNotePriorityId(model.NotePriorityId);
    setHideStartDate(model.HideStartDate);
    setHideExpiryDate(model.HideExpiryDate);
    setHideReminderDate(model.HideReminderDate);
    setStartDate(model.StartDate);
    setExpiryDate(model.ExpiryDate);
    setReminderDate(model.ReminderDate);
    setOwnerUserId(model.OwnerUserId);
    setRequestedByUserId(model.RequestedByUserId);
    setOwnerUserName(model.OwnerUserName);
    setIsSubmitButtonVisible(model.IsSubmitButtonVisible);
    setIsVersioningButtonVisible(model.IsVersioningButtonVisible);
    setHidePriority(model.HidePriority);
    setNoteId(model.NoteId);
    setIsDraftButtonVisible(model.IsDraftButtonVisible);
    setSaveAsDraftText(model.SaveAsDraftText);
    setNoteStatusName(model.NoteStatusName);
    setVersionNo(model.VersionNo);
    setCompleteButtonText(model.CompleteButtonText);
    setParentNoteId(model.ParentNoteId);
    setParentTaskId(model.ParentTaskId);
    setParentServiceId(model.ParentServiceId);
    setCreatedBy(model.CreatedBy);
    setLastUpdatedBy(model.LastUpdatedBy);
    setLegalEntityId(model.LegalEntityId);
    setCompanyId(model.CompanyId);
    setReferenceType(model.ReferenceType);
    setReferenceId(model.ReferenceId);
    setServicePlusId(model.ServicePlusId);
    setNotePlusId(model.NotePlusId);
    setTaskPlusId(model.TaskPlusId);
    setNoteStatusCode(model.NoteStatusCode);
    setPortalId(model.PortalId);
    

  }
  const OnSubmit = async () => {
    alert(NoteNo);
    const json = "{ LocationName: 'll16', LocationDescription: 'l1desc', LocationNameArabic: 'lla16' }";
    const data = {
      Id: Id,
      TemplateCode: TemplateCode,
      TemplateId: TemplateId,
      NoteTemplateId: NoteTemplateId,
      TableMetadataId: TableMetadataId,
      NoteNo: NoteNo,
      NoteSubject: NoteSubject,
      NoteDescription: NoteDescription,
      SequenceOrder: SequenceOrder,
      NotePriorityId: NotePriorityId,
      StartDate: StartDate,
      ExpiryDate: ExpiryDate,
      ReminderDate: ReminderDate,
      RequestedByUserId: RequestedByUserId,
      OwnerUserId: OwnerUserId,  
      NoteId: NoteId,
      ParentNoteId: ParentNoteId,
      ParentTaskId: ParentTaskId,
      ParentServiceId: ParentServiceId,
      VersionNo: VersionNo,
      CreatedBy: CreatedBy,
      LastUpdatedBy: LastUpdatedBy,
      LegalEntityId: LegalEntityId,
      CompanyId: CompanyId,
      ReferenceType: ReferenceType,
      ReferenceId: ReferenceId,
      ServicePlusId: ServicePlusId,
      NotePlusId: NotePlusId,
      TaskPlusId: TaskPlusId,
      PortalId: PortalId,
      NoteStatusCode: 'NOTE_STATUS_INPROGRESS',
      DataAction:'Create',
      Json: json
    };


    // await axios
    //   .post(manageURL, data)
    //   .then((response) => {
    //     alert("Data added!");
    //     alert(response.data.IsSuccess);
    //     setOpen(false);
    //     navigate("/read");
    //     closeModal(false)

    //   }).catch(error => {
    //     alert("error===" + error);
    //   });

    try {
      const response = await axios.post(manageURL, data);
      alert("Data Added");
      alert(response.data.IsSuccess);
      // console.log(response.data)
      closeModal(false);
     await   viewData()
    } catch (error) {
      console.log(error.message);
    }
  }

  useEffect(() => {
    viewData()
  }, [])

  return (

    <Card>

      <Box sx={{ height: 600, width: 850, background: 'white', transform: 'translateZ(0px)', flexGrow: 1 }}>

        <Box sx={{ width: 850, p: 5, pb: 3, alignItems: 'right' }}>

          <Box sx={{ textAlign: 'right' }}>
            <IconButton aria-label="Attach" sx={{ background: '#f7c46c', color: 'white', p: 2, m: 1 }}>
              <AttachFileRoundedIcon />
            </IconButton>
            <IconButton aria-label="Share" sx={{ background: '#00a28a', color: 'white', p: 2, m: 1 }} >
              <NoteShare />
            </IconButton>
            <IconButton aria-label="Comment" sx={{ background: '#0179a8', color: 'white', p: 2, m: 1 }}>
              <TextsmsRoundedIcon />
            </IconButton>
            <IconButton aria-label="More" sx={{ background: '#9155FD', color: 'white', padding: 0, m: 1 }}>
              <MoreDropdown />
            </IconButton>
          </Box>


        </Box>
        <div>
          <Box
            sx={{
              flexGrow: 1,
              display: "flex",
              flexDirection: "column",
              p: 1,
              "& .MuiTextField-root": { p: 1.2 },
              "& .MuiFormControl-root": { p: 1.2 },
            }}
          >

            <NoteHeader UserName={OwnerUserName} NoteNo={NoteNo} NoteStatus={NoteStatusName} VersionNo={VersionNo} />

            {IsInEditMode &&
              <Grid container spacing={6}>
                {NumberGenerationType == 1 &&
                  <Grid item xs={12}>

                    <TextField
                      fullWidth
                      id="outlined-required"
                      label={NoteNoText ? NoteNoText : 'Note No'}
                      Value={NoteNo}
                      onChange={event => setNoteNo(event.target.value)}
                      sx={{ marginBottom: "8px" }}
                    />
                  </Grid>
                }
                {!HideHeader && !HideSubject &&

                  <Grid item xs={12}>
                    <TextField
                      fullWidth
                      id="outlined-required"
                      label={SubjectText ? SubjectText : 'Subject'}
                      Value={NoteSubject}
                      onChange={event => setNoteSubject(event.target.value)}
                      sx={{ marginBottom: "8px" }}
                    />
                  </Grid>

                }
                {!HideHeader && !HideDescription &&

                  <Grid item xs={12}>
                    <TextField
                      fullWidth
                      id="outlined-required"
                      label={DescriptionText ? DescriptionText : 'Description'}
                      Value={NoteDescription}
                      onChange={event => setNoteDescription(event.target.value)}
                      sx={{ marginBottom: "8px" }}
                    />
                  </Grid>
                }
                {EnableSequenceOrder &&
                  <Grid item xs={12}>
                    <TextField
                      fullWidth
                      id="outlined-required"
                      label="Sequence Order"
                      Value={SequenceOrder}
                      onChange={event => setSequenceOrder(event.target.value)}
                      type="number"
                      sx={{ marginBottom: "8px" }}
                    />
                  </Grid>
                }
                {!HideHeader && !HideStartDate &&
                  <Grid item sm={4} xs={12}>
                    <TextField
                      fullWidth
                      sx={{ marginBottom: "8px" }}
                      id="date"
                      label="Start Date"
                      Value={StartDate}
                      type="date"
                      onChange={event => setStartDate(event.target.value)}
                      defaultValue="YYYY-MM-DD"
                      InputLabelProps={{
                        shrink: true,
                      }}
                    />
                  </Grid>
                }
                {!HideHeader && !HideExpiryDate &&
                  <Grid item sm={4} xs={12}>
                    <TextField
                      fullWidth
                      sx={{ marginBottom: "8px" }}
                      id="date"
                      label="Expiry Date"
                      Value={ExpiryDate}
                      type="date"
                      onChange={event => setExpiryDate(event.target.value)}
                      defaultValue="YYYY-MM-DD"
                      InputLabelProps={{
                        shrink: true,
                      }}
                    />
                  </Grid>
                }
                {!HideHeader && !HideReminderDate &&
                  <Grid item sm={4} xs={12}>
                    <TextField
                      fullWidth
                      sx={{ marginBottom: "8px" }}
                      id="date"
                      label="Reminder Date"
                      Value={ReminderDate}
                      type="date"
                      onChange={event => setReminderDate(event.target.value)}
                      defaultValue="YYYY-MM-DD"
                      InputLabelProps={{
                        shrink: true,
                      }}
                    />
                  </Grid>
                }

              </Grid>
            }
          </Box>

          <Box sx={{ '& button': { m: 2 }, textAlign: 'right' }}>


            {IsDraftButtonVisible &&

              <Button variant="contained" size="medium">
                {SaveAsDraftText ? SaveAsDraftText : 'Save As Draft'}
              </Button>
            }

            {IsSubmitButtonVisible &&
              <Button variant="contained" size="medium" onClick={OnSubmit}>
                {CompleteButtonText ? CompleteButtonText : 'Submit'}
              </Button>
            }
          </Box>



        </div>
      </Box>
      <Dialog
        fullWidth
        open={showShare}
        maxWidth='md'
        scroll='body'
        onClose={() => setShowShare(false)}
        TransitionComponent={Transition}
        onBackdropClick={() => setShowShare(false)}
      >
        <NoteShare />
      </Dialog>
    </Card>
  )
}

export default Note





