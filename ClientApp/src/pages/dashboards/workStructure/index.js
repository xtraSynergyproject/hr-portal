
// ** React Imports
import { useState, useEffect, useCallback } from 'react'

// ** Next Imports
import Link from 'next/link'

// ** MUI Imports
import Box from '@mui/material/Box'
import Card from '@mui/material/Card'
import Menu from '@mui/material/Menu'
import Grid from '@mui/material/Grid'
import Divider from '@mui/material/Divider'
import { DataGrid } from '@mui/x-data-grid'
import { styled } from '@mui/material/styles'
import MenuItem from '@mui/material/MenuItem'
import IconButton from '@mui/material/IconButton'
import Typography from '@mui/material/Typography'
import CardHeader from '@mui/material/CardHeader'
import InputLabel from '@mui/material/InputLabel'
import FormControl from '@mui/material/FormControl'
import CardContent from '@mui/material/CardContent'
import Select from '@mui/material/Select'

// ** Icon Imports
import Icon from 'src/@core/components/icon'

// ** Store Imports
import { useDispatch, useSelector } from 'react-redux'

// ** Custom Components Imports
import CustomChip from 'src/@core/components/mui/chip'
import CustomAvatar from 'src/@core/components/mui/avatar'
import CardStatisticsHorizontal from 'src/@core/components/card-statistics/card-stats-horizontal'

// ** Utils Import
import { getInitials } from 'src/@core/utils/get-initials'

// ** Actions Imports
import { fetchData, deleteUser } from 'src/store/apps/user'

// ** Third Party Components
import axios from 'axios'

// ** Custom Table Components Imports
import TableHeader from 'src/views/apps/user/list/TableHeader'
import AddUserDrawer from 'src/views/apps/user/list/AddUserDrawer'

function createData(
NoteSubject,
StartDate,
CompletedDate,
TemplateId,
RequestedByUserId,
ParentTaskId,
UserRating,
LegalEntityId,
// TemplateCode,
ReminderDate,
NotePriorityId,
NoteOwnerTypeId,
ParentNoteId,
CompleteReason,
ClosedDate,
SequenceOrder,
NoteStatusId,
NoteTemplateId,
OwnerUserId,
CancelReason,
CloseComment,
NoteId,
CreatedDate,
CreatedBy,
LastUpdatedDate,
LastUpdatedBy,
IsDeleted,
CompanyId,
NoteDescription,
NoteNo,
ExpiryDate,
ParentServiceId,
NoteActionId,
IsVersioning,CanceledDate,
ReferenceType,
IsArchived,
LockStatus,
LastLockedDate,
ReferenceId,
DisablePermissionInheritance,
NoteEventId,
ServicePlusId,
NotePlusId,
TaskPlusId,
Id,
EnableIndexPage,
EnableNoteNumberManual,
EnableSaveAsDraft,
SaveAsDraftText,
SaveAsDraftCss,
CompleteButtonText,
CompleteButtonCss,
EnableBackButton,
BackButtonText ,BackButtonCss,
EnableAttachment,
EnableComment,
DisableVersioning,
SaveNewVersionButtonText,
SaveNewVersionButtonCss,
NoteIndexPageTemplateId,
CreateReturnType, 
EditReturnType, 
PreScript, 
PostScript, 
IconFileId,
BannerFileId, 
BackgroundFileId, 
Subject, 
NotificationSubject, 
Description, 
SubjectText,
OwnerUserText,
RequestedByUserText,
PriorityId, 
EnableCancelButton, 
IsCancelReasonRequired,
CancelButtonText,
CancelButtonCss,
IsUdfTemplate,
IsCompleteReasonRequired, 
NoteNoText,
DescriptionText, 
HideHeader,
HideSubject, 
HideDescription, 
HideStartDate, 
HideExpiryDate, 
HidePriority, 
IsSubjectMandatoIsDescriptionMandatory,
HideToolbar,
HideBanner,
AllowPastStartDate,
TemplateColor, 
HideOwner,
IsSubjectUnique,
EnablePrintButton, 
PrintButtonText,
EnableDataPermission,
DataPermissionColumnId,
IsNumberNotMandatory,
NumberGenerationType,
EnableLegalEntityFilter,
EnablePermission, 
AdhocTaskTemplateIds,
AdhocNoteTemplateIds,
EnableInlineComment, 
AdhocServiceTemplateIds, 
OcrTemplateFileId, 
SubjectUdfMappingColumn,
DescriptionUdfMappingColumn,
LocalizedColumnId,
DisplayColumnId,
FormClientScript,
NoteTemplateType,
EmailCopyTemplateCode,
FormType,
SubjectMappingUdfId, 
ActionButtonPosition, 
EnableSequenceOrder, 
DescriptionMappingUdfId,
UdfNoteTableId,PolicyName, 
PolicyDescription, 
PolicyDocument, 
CreatedByUser_Id,TemplateCode,
CreatedByUser_Name,
CreatedByUser_Email,
CreatedByUser_JobTitle,
CreatedByUser_PhotoId,
UpdatedByUser_Id,
UpdatedByUser_Name,
UpdatedByUser_Email, 
UpdatedByUser_JobTitle,
UpdatedByUser_PhotoId, 
OwnerUserName,
OwnerUserEmail,
OwnerUserJobTitle, 
OwnerUserPhotoId, 
RequestedByUserName, 
RequestedByUserEmail,
RequestedByUserJobTitle,
RequestedByUserPhotoId, 
TableMetadataId, 
TemplateName, 
TemplateDisplayName, 
Json, 
NoteOwnerTypeCode, 
NotePriorityName, 
NotePriorityCode, 
NoteStatusName, 
StatusCode, 
NoteActionName) {
return {NoteSubject,
  StartDate,
  CompletedDate,
  TemplateId,
  RequestedByUserId,
  ParentTaskId,
  UserRating,
  LegalEntityId,
  TemplateCode,
  ReminderDate,
  NotePriorityId,
  NoteOwnerTypeId,
  ParentNoteId,
  CompleteReason,
  ClosedDate,
  SequenceOrder,                                                     
  NoteStatusId,                                                       
  NoteTemplateId,
  OwnerUserId,
  CancelReason,
  CloseComment,
  NoteId,
  CreatedDate,
  CreatedBy,
  LastUpdatedDate,
  LastUpdatedBy,
  IsDeleted,
  CompanyId,
  NoteDescription,
  NoteNo,
  ExpiryDate,
  ParentServiceId,
  NoteActionId,
  IsVersioning,CanceledDate,
  ReferenceType,
  IsArchived,
  LockStatus,
  LastLockedDate,
  ReferenceId,
  DisablePermissionInheritance,
  NoteEventId,
  ServicePlusId,
  NotePlusId,
  TaskPlusId,
  Id,
  EnableIndexPage,
  EnableNoteNumberManual,
  EnableSaveAsDraft,
  SaveAsDraftText,
  SaveAsDraftCss,
  CompleteButtonText,
  CompleteButtonCss,
  EnableBackButton,
  BackButtonText ,BackButtonCss,
  EnableAttachment,
  EnableComment,
  DisableVersioning,
  SaveNewVersionButtonText,
  SaveNewVersionButtonCss,
  NoteIndexPageTemplateId,
  CreateReturnType, 
  EditReturnType, 
  PreScript, 
  PostScript, 
  IconFileId,
  BannerFileId, 
  BackgroundFileId, 
  Subject, 
  NotificationSubject, 
  Description, 
  SubjectText,
  OwnerUserText,
  RequestedByUserText,
  PriorityId, 
  EnableCancelButton, 
  IsCancelReasonRequired,
  CancelButtonText,
  CancelButtonCss,
  IsUdfTemplate,
  IsCompleteReasonRequired, 
  NoteNoText,
  DescriptionText, 
  HideHeader,
  HideSubject, 
  HideDescription, 
  HideStartDate, 
  HideExpiryDate, 
  HidePriority, 
  IsSubjectMandatoIsDescriptionMandatory,
  HideToolbar,
  HideBanner,
  AllowPastStartDate,
  TemplateColor, 
  HideOwner,
  IsSubjectUnique,
  EnablePrintButton, 
  PrintButtonText,
  EnableDataPermission,
  DataPermissionColumnId,
  IsNumberNotMandatory,
  NumberGenerationType,
  EnableLegalEntityFilter,
  EnablePermission, 
  AdhocTaskTemplateIds,
  AdhocNoteTemplateIds,
  EnableInlineComment, 
  AdhocServiceTemplateIds, 
  OcrTemplateFileId, 
  SubjectUdfMappingColumn,
  DescriptionUdfMappingColumn,
  LocalizedColumnId,
  DisplayColumnId,
  FormClientScript,
  NoteTemplateType,
  EmailCopyTemplateCode,
  FormType,
  SubjectMappingUdfId, 
  ActionButtonPosition, 
  EnableSequenceOrder, 
  DescriptionMappingUdfId,
  UdfNoteTableId,PolicyName, 
  PolicyDescription, 
  PolicyDocument, 
  CreatedByUser_Id,
  CreatedByUser_Name,
  CreatedByUser_Email,
  CreatedByUser_JobTitle,
  CreatedByUser_PhotoId,
  UpdatedByUser_Id,
  UpdatedByUser_Name,
  UpdatedByUser_Email, 
  UpdatedByUser_JobTitle,
  UpdatedByUser_PhotoId, 
  OwnerUserName,
  OwnerUserEmail,
  OwnerUserJobTitle, 
  OwnerUserPhotoId, 
  RequestedByUserName, 
  RequestedByUserEmail,
  RequestedByUserJobTitle,
  RequestedByUserPhotoId, 
  TableMetadataId, 
  TemplateName, 
  TemplateDisplayName, 
  Json, 
  NoteOwnerTypeCode, 
  NotePriorityName, 
  NotePriorityCode, 
  NoteStatusName, 
  StatusCode, 
  NoteActionName};
 }

// ** Vars
const userRoleObj = {
  admin: { icon: 'mdi:laptop', color: 'error.main' },
  author: { icon: 'mdi:cog-outline', color: 'warning.main' },
  editor: { icon: 'mdi:pencil-outline', color: 'info.main' },
  maintainer: { icon: 'mdi:chart-donut', color: 'success.main' },
  subscriber: { icon: 'mdi:account-outline', color: 'primary.main' }
}

const userStatusObj = {
  active: 'success',
  pending: 'warning',
  inactive: 'secondary'
}

const StyledLink = styled(Link)(({ theme }) => ({
  fontWeight: 600,
  fontSize: '1rem',
  cursor: 'pointer',
  textDecoration: 'none',
  color: theme.palette.text.secondary,
  '&:hover': {
    color: theme.palette.primary.main
  }
}))

// ** renders client column
const renderClient = row => {
  // if (row.avatar.length) {
  //   return <CustomAvatar src={row.avatar} sx={{ mr: 3, width: 30, height: 30 }} />
  // } else {
  //   return (
  //     <CustomAvatar
  //       skin='light'
  //       color={row.avatarColor || 'primary'}
  //       sx={{ mr: 3, width: 30, height: 30, fontSize: '.875rem' }}
  //     >
  //       {/* {getInitials(row.fullName ? row.fullName : 'John Doe')} */}
  //     </CustomAvatar>
  //   )
  // }

  
}

const RowOptions = ({ id }) => {
  // ** Hooks
  const dispatch = useDispatch()

  // ** State
  const [anchorEl, setAnchorEl] = useState(null)
  const rowOptionsOpen = Boolean(anchorEl)

  const handleRowOptionsClick = event => {
    setAnchorEl(event.currentTarget)
  }

  const handleRowOptionsClose = () => {
    setAnchorEl(null)
  }

  const handleDelete = () => {
    dispatch(deleteUser(id))
    handleRowOptionsClose()
  }

  return (
    <>
      <IconButton size='small' onClick={handleRowOptionsClick}>
        <Icon icon='mdi:dots-vertical' />
      </IconButton>
      <Menu
        keepMounted
        anchorEl={anchorEl}
        open={rowOptionsOpen}
        onClose={handleRowOptionsClose}
        anchorOrigin={{
          vertical: 'bottom',
          horizontal: 'right'
        }}
        transformOrigin={{
          vertical: 'top',
          horizontal: 'right'        }}
        PaperProps={{ style: { minWidth: '8rem' } }}
      >
        <MenuItem
          component={Link}
          sx={{ '& svg': { mr: 2 } }}
          onClick={handleRowOptionsClose}
          href='/apps/user/view/overview/'
        >
          <Icon icon='mdi:eye-outline' fontSize={20} />
          View
        </MenuItem>
        <MenuItem onClick={handleRowOptionsClose} sx={{ '& svg': { mr: 2 } }}>
          <Icon icon='mdi:pencil-outline' fontSize={20} />
          Edit
        </MenuItem>
        <MenuItem onClick={handleDelete} sx={{ '& svg': { mr: 2 } }}>
          <Icon icon='mdi:delete-outline' fontSize={20} />
          Delete
        </MenuItem>
      </Menu>
    </>
  )
}

const columns = [
  {
    flex: 0.2,
    minWidth: 250,
    field: 'NoteSubject',
    headerName: 'Note Subject',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.NoteSubject}
        </Typography>
      )
    }
  },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'StartDate',
    headerName: 'Start Date',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.StartDate}
        </Typography>
      )
    }
  },
  {
    flex: 0.15,
    field: 'CompletedDate',
    minWidth: 150,
    headerName: 'Completed Date',
    renderCell: ({ row }) => {
      return (
        <Box sx={{ display: 'flex', alignItems: 'center'}}>
          <Icon fontSize={20} />
          <Typography noWrap sx={{ color: 'text.secondary', textTransform: 'capitalize' }}>
            {row.CompletedDate}
          </Typography>
        </Box>
      )
    }
  },
  {
    flex: 0.15,
    minWidth: 120,
    headerName: 'Template Id',
    field: 'TemplateId',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap sx={{ textTransform: 'capitalize' }}>
          {row.TemplateId}
        </Typography>
      )
    }
  },
  {
    flex: 0.15,
    minWidth: 120,
    headerName: 'Requested By User Id',
    field: 'RequestedByUserId',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap sx={{ textTransform: 'capitalize' }}>
          {row.RequestedByUserId}
        </Typography>
      )
    }
  },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'ParentTaskId',
    headerName: 'Parent Task Id',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.ParentTaskId}
        </Typography>
      )
    }
  },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'UserRating',
    headerName: 'User Rating',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.UserRating}
        </Typography>
      )
    }
  },
  {
    flex: 0.15,
    field: 'LegalEntityId',
    minWidth: 150,
    headerName: 'Legal Entity Id',
    renderCell: ({ row }) => {
      return (
        <Box sx={{ display: 'flex', alignItems: 'center'}}>
          <Icon fontSize={20} />
          <Typography noWrap sx={{ color: 'text.secondary', textTransform: 'capitalize' }}>
            {row.LegalEntityId}
          </Typography>
        </Box>
      )
    }
  },
  // {
  //   flex: 0.15,
  //   minWidth: 120,
  //   headerName: 'Template Code',
  //   field: 'TemplateCode',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Typography noWrap sx={{ textTransform: 'capitalize' }}>
  //         {row.TemplateCode}
  //       </Typography>
  //     )
  //   }
  // }
  {
    flex: 0.15,
    minWidth: 120,
    headerName: 'Reminder Date',
    field: 'ReminderDate',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap sx={{ textTransform: 'capitalize' }}>
          {row.ReminderDate}
        </Typography>
      )
    }
  },
  
  {
    flex: 0.2,
    minWidth: 250,
    field: 'NotePriorityId',
    headerName: 'Note Priority Id',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.NotePriorityId}
        </Typography>
      )
    }
  },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'NoteOwnerTypeId',
    headerName: 'Note Owner Type Id',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.NoteOwnerTypeId}
        </Typography>
      )
    }
  },
  {
    flex: 0.15,
    field: 'ParentNoteId',
    minWidth: 150,
    headerName: 'Parent Note Id',
    renderCell: ({ row }) => {
      return (
        <Box sx={{ display: 'flex', alignItems: 'center'}}>
          <Icon fontSize={20} />
          <Typography noWrap sx={{ color: 'text.secondary', textTransform: 'capitalize' }}>
            {row.ParentNoteId}
          </Typography>
        </Box>
      )
    }
  },
  {
    flex: 0.15,
    minWidth: 120,
    headerName: 'Complete Reason',
    field: 'CompleteReason',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap sx={{ textTransform: 'capitalize' }}>
          {row.CompleteReason}
        </Typography>
      )
    }
  },
  {
    flex: 0.15,
    minWidth: 120,
    headerName: 'Closed Date',
    field: 'ClosedDate',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap sx={{ textTransform: 'capitalize' }}>
          {row.ClosedDate}
        </Typography>
      )
    }
  },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'SequenceOrder',
    headerName: 'Sequence Order',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.SequenceOrder}
        </Typography>
      )
    }
  },
  {
    flex: 0.15,
    field: 'NoteStatusId',
    minWidth: 150,
    headerName: 'Note Status Id',
    renderCell: ({ row }) => {
      return (
        <Box sx={{ display: 'flex', alignItems: 'center'}}>
          <Icon fontSize={20} />
          <Typography noWrap sx={{ color: 'text.secondary', textTransform: 'capitalize' }}>
            {row.NoteStatusId}
          </Typography>
        </Box>
      )
    }
  },
  {
    flex: 0.15,
    minWidth: 120,
    headerName: 'Note Template Id',
    field: 'NoteTemplateId',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap sx={{ textTransform: 'capitalize' }}>
          {row.NoteTemplateId}
        </Typography>
      )
    }
  },
  {
    flex: 0.15,
    minWidth: 120,
    headerName: 'Owner User Id',
    field: 'OwnerUserId',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap sx={{ textTransform: 'capitalize' }}>
          {row.OwnerUserId}
        </Typography>
      )
    }
  },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'CancelReason',
    headerName: 'Cancel Reason',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.CancelReason}
        </Typography>
      )
    }
  },

  {
    flex: 0.2,
    minWidth: 250,
    field: 'CloseComment',
    headerName: 'Close Comment',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.CloseComment}
        </Typography>
      )
    }
  },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'NoteId',
    headerName: 'Note Id',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.NoteId}
        </Typography>
      )
    }
  },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'CreatedDate',
    headerName: 'Created Date',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.CreatedDate}
        </Typography>
      )
    }
  },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'CreatedBy',
    headerName: 'Created By',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.CreatedBy}
        </Typography>
      )
    }
  },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'LastUpdatedDate',
    headerName: 'Last Updated Date',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.LastUpdatedDate}
        </Typography>
      )
    }
  },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'LastUpdatedBy',
    headerName: 'Last Updated By',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.LastUpdatedBy}
        </Typography>
      )
    }
  },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'IsDeleted',
    headerName: 'IsDeleted',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.IsDeleted}
        </Typography>
      )
    }
  },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'CompanyId',
    headerName: 'Company Id',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.CompanyId}
        </Typography>
      )
    }
  },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'Status',
    headerName: 'Status',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.Status}
        </Typography>
      )
    }
  },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'VersionNo',
    headerName: 'VersionNo',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.VersionNo}
        </Typography>
      )
    }
  },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'NoteDescription',
    headerName: 'Note Description',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.NoteDescription}
        </Typography>
      )
    }
  },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'NoteNo',
    headerName: 'Note No',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.NoteNo}
        </Typography>
      )
    }
  },

  {
    flex: 0.2,
    minWidth: 250,
    field: 'ExpiryDate',
    headerName: 'Expiry Date',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.ExpiryDate}
        </Typography>
      )
    }
  },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'ParentServiceId',
    headerName: 'Parent Service Id',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.ParentServiceId}
        </Typography>
      )
    }
  },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'NoteActionId',
    headerName: 'Note Action Id',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.NoteActionId}
        </Typography>
      )
    }
  },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'IsVersioning',
    headerName: 'IsVersioning',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.IsVersioning}
        </Typography>
      )
    }
  },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'CanceledDate',
    headerName: 'Canceled Date',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.CanceledDate}
        </Typography>
      )
    }
  },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'ReferenceType',
    headerName: 'Reference Type',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.ReferenceType}
        </Typography>
      )
    }
  },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'IsArchived',
    headerName: 'IsArchived',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.IsArchived}
        </Typography>
      )
    }
  },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'LockStatus',
    headerName: 'Lock Status',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.LockStatus}
        </Typography>
      )
    }
  },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'LastLockedDate',
    headerName: 'Last Locked Date',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.LastLockedDate}
        </Typography>
      )
    }
  },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'ReferenceId',
    headerName: 'Reference Id',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.ReferenceId}
        </Typography>
      )
    }
  },
 
  {
    flex: 0.2,
    minWidth: 250,
    field: 'DisablePermissionInheritance',
    headerName: 'Disable Permission Inheritance',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.DisablePermissionInheritance}
        </Typography>
      )
    }
  },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'NoteEventId',
    headerName: 'Note Event Id',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.NoteEventId}
        </Typography>
      )
    }
  },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'ServicePlusId',
    headerName: 'Service Plus Id',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.ServicePlusId}
        </Typography>
      )
    }
  },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'NotePlusId',
    headerName: 'Note Plus Id',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.NotePlusId}
        </Typography>
      )
    }
  },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'TaskPlusId',
    headerName: 'Task Plus Id',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.TaskPlusId}
        </Typography>
      )
    }
  },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'Id',
    headerName: 'Id',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.Id}
        </Typography>
      )
    }
  },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'EnableIndexPage',
    headerName: 'Enable Index Page',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.EnableIndexPage}
        </Typography>
      )
    }
  },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'EnableNoteNumberManual',
    headerName: 'Enable Note Number Manual',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.EnableNoteNumberManual}
        </Typography>
      )
    }
  },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'EnableSaveAsDraft',
    headerName: 'Enable Save As Draft',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.EnableSaveAsDraft}
        </Typography>
      )
    }
  },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'SaveAsDraftText',
    headerName: 'Save As Draft Text',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.SaveAsDraftText}
        </Typography>
      )
    }
  },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'SaveAsDraftCss',
    headerName: 'Save As Draft Css',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.SaveAsDraftCss}
        </Typography>
      )
    }
  },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'CompleteButtonText',
    headerName: 'Complete Button Text',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.CompleteButtonText}
        </Typography>
      )
    }
  },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'CompleteButtonCss',
    headerName: 'Complete Button Css',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.CompleteButtonCss}
        </Typography>
      )
    }
  },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'EnableBackButton',
    headerName: 'Enable Back Button',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.EnableBackButton}
        </Typography>
      )
    }
  },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'BackButtonText ',
    headerName: 'Back Button Text ',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.BackButtonText }
        </Typography>
      )
    }
  },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'BackButtonCss',
    headerName: 'Back Button Css',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.BackButtonCss}
        </Typography>
      )
    }
  },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'EnableAttachment',
    headerName: 'Enable Attachment',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.EnableAttachment}
        </Typography>
      )
    }
  },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'EnableComment',
    headerName: 'Enable Comment',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.EnableComment}
        </Typography>
      )
    }
  },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'DisableVersioning',
    headerName: 'Disable Versioning',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.DisableVersioning}
        </Typography>
      )
    }
  },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'SaveNewVersionButtonText',
    headerName: 'Save New Version Button Text',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.SaveNewVersionButtonText}
        </Typography>
      )
    }
  },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'SaveNewVersionButtonCss',
    headerName: 'Save New Version Button Css',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.SaveNewVersionButtonCss}
        </Typography>
      )
    }
  },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'NoteIndexPageTemplateId',
    headerName: 'Note Index Page Template Id',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.NoteIndexPageTemplateId}
        </Typography>
      )
    }
  },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'CreateReturnType',
    headerName: 'Create Return Type',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.CreateReturnType}
        </Typography>
      )
    }
  },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'EditReturnType',
    headerName: 'Edit Return Type',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.EditReturnType}
        </Typography>
      )
    }
  },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'PreScript',
    headerName: 'PreScript',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.PreScript}
        </Typography>
      )
    }
  },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'PostScript',
    headerName: 'Post Script',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.PostScript}
        </Typography>
      )
    }
  },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'IconFileId',
    headerName: 'Icon File Id',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.IconFileId}
        </Typography>
      )
    }
  },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'BannerFileId',
    headerName: 'Banner File Id',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.BannerFileId}
        </Typography>
      )
    }
  },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'Background File Id',
    headerName: 'BackgroundFileId',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.BackgroundFileId}
        </Typography>
      )
    }
  },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'Subject',
    headerName: 'Subject',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.Subject}
        </Typography>
      )
    }
  },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'NotificationSubject',
    headerName: 'Notification Subject',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.NotificationSubject}
        </Typography>
      )
    }
  },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'Description',
    headerName: 'Description',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.Description}
        </Typography>
      )
    }
  },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'SubjectText',
    headerName: 'Subject Text',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.SubjectText}
        </Typography>
      )
    }
  },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'OwnerUserText',
    headerName: 'Owner User Text',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.OwnerUserText}
        </Typography>
      )
    }
  },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'RequestedByUserText',
    headerName: 'Requested By User Text',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.RequestedByUserText}
        </Typography>
      )
    }
  },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'PriorityId',
    headerName: 'Priority Id',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.PriorityId}
        </Typography>
      )
    }
  },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'EnableCancelButton',
    headerName: 'Enable Cancel Button',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.EnableCancelButton}
        </Typography>
      )
    }
  },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'IsCancelReasonRequired',
    headerName: 'IsCancel Reason Required',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.IsCancelReasonRequired}
        </Typography>
      )
    }
  },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'CancelButtonText',
    headerName: 'Cancel Button Text',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.CancelButtonText}
        </Typography>
      )
    }
  },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'CancelButtonCss',
    headerName: 'Cancel Button Css',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.CancelButtonCss}
        </Typography>
      )
    }
  },
 
  {
    flex: 0.2,
    minWidth: 250,
    field: 'IsUdfTemplate',
    headerName: 'IsUdf Template',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.IsUdfTemplate}
        </Typography>
      )
    }
  },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'IsCompleteReasonRequired',
    headerName: 'IsComplete Reason Required',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.IsCompleteReasonRequired}
        </Typography>
      )
    }
  },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'NoteNoText',
    headerName: 'Note No Text',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.NoteNoText}
        </Typography>
      )
    }
  },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'DescriptionText',
    headerName: 'Description Text',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.DescriptionText}
        </Typography>
      )
    }
  },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'HideHeader',
    headerName: 'Hide Header',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.HideHeader}
        </Typography>
      )
    }
  },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'HideSubject',
    headerName: 'Hide Subject',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.HideSubject}
        </Typography>
      )
    }
  },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'HideDescription',
    headerName: 'Hide Description',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.HideDescription}
        </Typography>
      )
    }
  },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'HideStartDate',
    headerName: 'Hide Start Date',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.HideStartDate}
        </Typography>
      )
    }
  },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'HideExpiryDate',
    headerName: 'Hide Expiry Date',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.HideExpiryDate}
        </Typography>
      )
    }
  },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'HidePriority',
    headerName: 'Hide Priority',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.HidePriority}
        </Typography>
      )
    }
  },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'IsSubjectMandatoIsDescriptionMandatory',
    headerName: 'IsSubject Mandato Is Description Mandatory',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.IsSubjectMandatoIsDescriptionMandatory}
        </Typography>
      )
    }
  },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'HideToolbar',
    headerName: 'Hide Toolbar',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.HideToolbar}
        </Typography>
      )
    }
  },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'HideBanner',
    headerName: 'Hide Banner',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.HideBanner}
        </Typography>
      )
    }
  },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'AllowPastStartDate',
    headerName: 'Allow Past Start Date',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.AllowPastStartDate}
        </Typography>
      )
    }
  },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'TemplateColor',
    headerName: 'Template Color',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.TemplateColor}
        </Typography>
      )
    }
  },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'HideOwner',
    headerName: 'Hide Owner',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.HideOwner}
        </Typography>
      )
    }
  },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'IsSubjectUnique',
    headerName: 'IsSubject Unique',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.IsSubjectUnique}
        </Typography>
      )
    }
  },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'EnablePrintButton',
    headerName: 'Enable Print Button',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.EnablePrintButton}
        </Typography>
      )
    }
  },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'PrintButtonText',
    headerName: 'Print Button Text',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.PrintButtonText}
        </Typography>
      )
    }
  },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'EnableDataPermission',
    headerName: 'Enable Data Permission',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.EnableDataPermission}
        </Typography>
      )
    }
  },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'DataPermissionColumnId',
    headerName: 'Data Permission Column Id',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.DataPermissionColumnId}
        </Typography>
      )
    }
  },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'IsNumberNotMandatory',
    headerName: 'IsNumber Not Mandatory',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.IsNumberNotMandatory}
        </Typography>
      )
    }
  },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'NumberGenerationType',
    headerName: 'Number Generation Type',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.NumberGenerationType}
        </Typography>
      )
    }
  },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'EnableLegalEntityFilter',
    headerName: 'Enable Legal Entity Filter',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.EnableLegalEntityFilter}
        </Typography>
      )
    }
  },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'EnablePermission',
    headerName: 'Enable Permission',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.EnablePermission}
        </Typography>
      )
    }
  },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'AdhocTaskTemplateIds',
    headerName: 'Adhoc Task Template Ids',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.AdhocTaskTemplateIds}
        </Typography>
      )
    }
  },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'AdhocNoteTemplateIds',
    headerName: 'Adhoc Note Template Ids',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.AdhocNoteTemplateIds}
        </Typography>
      )
    }
  },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'EnableInlineComment',
    headerName: 'Enable Inline Comment',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.EnableInlineComment}
        </Typography>
      )
    }
  },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'AdhocServiceTemplateIds',
    headerName: 'Adhoc Service Template Ids',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.AdhocServiceTemplateIds}
        </Typography>
      )
    }
  },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'OcrTemplateFileId',
    headerName: 'OcrTemplate File Id',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.OcrTemplateFileId}
        </Typography>
      )
    }
  },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'SubjectUdfMappingColumn',
    headerName: 'Subject Udf Mapping Column',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.SubjectUdfMappingColumn}
        </Typography>
      )
    }
  },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'DescriptionUdfMappingColumn',
    headerName: 'Description Udf Mapping Column',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.DescriptionUdfMappingColumn}
        </Typography>
      )
    }
  },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'LocalizedColumnId',
    headerName: 'Localized Column Id',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.LocalizedColumnId}
        </Typography>
      )
    }
  },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'DisplayColumnId',
    headerName: 'Display Column Id',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.DisplayColumnId}
        </Typography>
      )
    }
  },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'FormClientScript',
    headerName: 'Form Client Script',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.FormClientScript}
        </Typography>
      )
    }
  },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'NoteTemplateType',
    headerName: 'Note Template Type',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.NoteTemplateType}
        </Typography>
      )
    }
  },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'EmailCopyTemplateCode',
    headerName: 'Email Copy Template Code',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.EmailCopyTemplateCode}
        </Typography>
      )
    }
  },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'FormType',
    headerName: 'Form Type',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.FormType}
        </Typography>
      )
    }
  },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'SubjectMappingUdfId',
    headerName: 'Subject Mapping UdfId',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.SubjectMappingUdfId}
        </Typography>
      )
    }
  },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'ActionButtonPosition',
    headerName: 'Action Button Position',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.ActionButtonPosition}
        </Typography>
      )
    }
  },

  {
    flex: 0.2,
    minWidth: 250,
    field: 'EnableSequenceOrder',
    headerName: 'Enable Sequence Order',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.EnableSequenceOrder}
        </Typography>
      )
    }
  },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'DescriptionMappingUdfId',
    headerName: 'Description Mapping UdfId',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.DescriptionMappingUdfId}
        </Typography>
      )
    }
  },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'UdfNoteTableId',
    headerName: 'UdfNote Table Id',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.UdfNoteTableId}
        </Typography>
      )
    }
  },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'PolicyName',
    headerName: 'Policy Name',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.PolicyName}
        </Typography>
      )
    }
  },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'PolicyDescription',
    headerName: 'Policy Description',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.PolicyDescription}
        </Typography>
      )
    }
  },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'PolicyDocument',
    headerName: 'Policy Document',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.PolicyDocument}
        </Typography>
      )
    }
  },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'CreatedByUser_Id',
    headerName: 'Created By User Id',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.CreatedByUser_Id}
        </Typography>
      )
    }
  },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'CreatedByUser_Name',
    headerName: 'Created By User Name',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.CreatedByUser_Name}
        </Typography>
      )
    }
  },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'CreatedByUser_Email',
    headerName: 'Created By User Email',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.CreatedByUser_Email}
        </Typography>
      )
    }
  },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'CreatedByUser_JobTitle',
    headerName: 'Created By User JobTitle',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.CreatedByUser_JobTitle}
        </Typography>
      )
    }
  },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'CreatedByUser_PhotoId',
    headerName: 'Created By User Photo Id',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.CreatedByUser_PhotoId}
        </Typography>
      )
    }
  },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'UpdatedByUser_Id',
    headerName: 'Updated By User Id',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.UpdatedByUser_Id}
        </Typography>
      )
    }
  },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'UpdatedByUser_Name',
    headerName: 'Updated By User Name',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.UpdatedByUser_Name}
        </Typography>
      )
    }
  },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'UpdatedByUser_Email',
    headerName: 'Updated By User Email',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.UpdatedByUser_Email}
        </Typography>
      )
    }
  },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'UpdatedByUser_JobTitle',
    headerName: 'UpdatedByUser JobTitle',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.UpdatedByUser_JobTitle}
        </Typography>
      )
    }
  },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'UpdatedByUser_PhotoId',
    headerName: 'UpdatedByUser PhotoId',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.UpdatedByUser_PhotoId}
        </Typography>
      )
    }
  },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'OwnerUserName',
    headerName: 'Owner User Name',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.OwnerUserName}
        </Typography>
      )
    }
  },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'OwnerUserEmail',
    headerName: 'Owner User Email',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.OwnerUserEmail}
        </Typography>
      )
    }
  },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'OwnerUserJobTitle',
    headerName: 'Owner User Job Title',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.OwnerUserJobTitle}
        </Typography>
      )
    }
  },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'OwnerUserPhotoId',
    headerName: 'Owner User Photo Id',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.OwnerUserPhotoId}
        </Typography>
      )
    }
  },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'RequestedByUserName',
    headerName: 'Requested By User Name',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.RequestedByUserName}
        </Typography>
      )
    }
  },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'RequestedByUserEmail',
    headerName: 'Requested By User Email',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.RequestedByUserEmail}
        </Typography>
      )
    }
  },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'RequestedByUserJobTitle',
    headerName: 'Requested By User Job Title',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.RequestedByUserJobTitle}
        </Typography>
      )
    }
  },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'RequestedByUserPhotoId',
    headerName: 'Requested By User Photo Id',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.RequestedByUserPhotoId}
        </Typography>
      )
    }
  },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'TableMetadataId',
    headerName: 'Table Metadata Id',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.TableMetadataId}
        </Typography>
      )
    }
  },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'TemplateName',
    headerName: 'Template Name',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.TemplateName}
        </Typography>
      )
    }
  },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'TemplateDisplayName',
    headerName: 'Template Display Name',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.TemplateDisplayName}
        </Typography>
      )
    }
  },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'Json',
    headerName: 'Json',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.Json}
        </Typography>
      )
    }
  },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'NoteOwnerTypeCode',
    headerName: 'Note Owner Type Code',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.NoteOwnerTypeCode}
        </Typography>
      )
    }
  },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'NoteOwnerTypeName',
    headerName: 'Note Owner Type Name',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.NoteOwnerTypeName}
        </Typography>
      )
    }
  },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'NotePriorityCode',
    headerName: 'Note Priority Code',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.NotePriorityCode}
        </Typography>
      )
    }
  },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'NotePriorityName',
    headerName: 'Note Priority Name',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.NotePriorityName}
        </Typography>
      )
    }
  },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'StatusCode',
    headerName: 'Note Status Code',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.NoteStatusCode}
        </Typography>
      )
    }
  },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'NoteStatusName',
    headerName: 'Note Status Name',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.NoteStatusName}
        </Typography>
      )
    }
  },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'NoteActionCode',
    headerName: 'Note Action Code',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.NoteActionCode}
        </Typography>
      )
    }
  },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'NoteActionName',
    headerName: 'Note Action Name',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.NoteActionName}
        </Typography>
      )
    }
  },
 
  {
    flex: 0.1,
    minWidth: 90,
    sortable: false,
    field: 'actions',
    headerName: 'Actions',
    renderCell: ({ row }) => <RowOptions id={row.id} />
  }
]

const WorkStructureDashboard  = () => {
   // ** State
   const [plan, setPlan] = useState('')
   const [value, setValue] = useState('')
   const [pageSize, setPageSize] = useState(10)
 
   // Api Intregration by using Get method
    const [getdata, setGetdata] = useState([]) 
   const viewData = async () => {
     let response = await axios.get(`https://webapidev.aitalkx.com/CHR/query/LoadNoteIndexPageGrid?indexPageTemplateId=fa5e0566-156a-4570-9b03-f5fae83f2d05&userId=45bba746-3309-49b7-9c03-b5793369d73c`)
     setGetdata(response.data)
     //console.log(response.data, "response data")
   }
   console.log(getdata, "response")
 
   useEffect(() => {
     viewData()
   }, [])
   
 
   // ** Hooks
   // const dispatch = useDispatch()
   // const store = useSelector(state => state.user)
   // useEffect(() => {
   //   dispatch(
   //     fetchData({
   //       role: '',
   //       q: value,
   //       status: '',
   //       currentPlan: plan
   //     })
   //   )
   // }, [dispatch, plan, value])

   const handleFilter = useCallback(val => {
    setValue(val)
  }, [])

  const handlePlanChange = useCallback(e => {
    setPlan(e.target.value)
  }, [])


  return (
    <Grid container spacing={6}>
     
      <Grid item xs={12}>
        <Card>
          <CardHeader title='Work Structure' />
          <CardContent>
            <Grid container spacing={6}>
              {/* <Grid item sm={4} xs={12}>
                <FormControl fullWidth>
                  <InputLabel id='role-select'>Select Role</InputLabel>
                  <Select
                    fullWidth
                    value={role}
                    id='select-role'
                    label='Select Role'
                    labelId='role-select'
                    onChange={handleRoleChange}
                    inputProps={{ placeholder: 'Select Role' }}
                  >
                    <MenuItem value=''>Select Role</MenuItem>
                    <MenuItem value='admin'>Admin</MenuItem>
                    <MenuItem value='author'>Author</MenuItem>
                    <MenuItem value='editor'>Editor</MenuItem>
                    <MenuItem value='maintainer'>Maintainer</MenuItem>
                    <MenuItem value='subscriber'>Subscriber</MenuItem>
                  </Select>
                </FormControl>
              </Grid>
              <Grid item sm={4} xs={12}>
                <FormControl fullWidth>
                  <InputLabel id='plan-select'>Select Plan</InputLabel>
                  <Select
                    fullWidth
                    value={plan}
                    id='select-plan'
                    label='Select Plan'
                    labelId='plan-select'
                    onChange={handlePlanChange}
                    inputProps={{ placeholder: 'Select Plan' }}
                  >
                    <MenuItem value=''>Select Plan</MenuItem>
                    <MenuItem value='basic'>Basic</MenuItem>
                    <MenuItem value='company'>Company</MenuItem>
                    <MenuItem value='enterprise'>Enterprise</MenuItem>
                    <MenuItem value='team'>Team</MenuItem>
                  </Select>
                </FormControl>
              </Grid>
              <Grid item sm={4} xs={12}>
                <FormControl fullWidth>
                  <InputLabel id='status-select'>Select Status</InputLabel>
                  <Select
                    fullWidth
                    value={status}
                    id='select-status'
                    label='Select Status'
                    labelId='status-select'
                    onChange={handleStatusChange}
                    inputProps={{ placeholder: 'Select Role' }}
                  >
                    <MenuItem value=''>Select Role</MenuItem>
                    <MenuItem value='pending'>Pending</MenuItem>
                    <MenuItem value='active'>Active</MenuItem>
                    <MenuItem value='inactive'>Inactive</MenuItem>
                  </Select>
                </FormControl>
              </Grid> */}
            </Grid>
          </CardContent>
          <Divider />
          <TableHeader value={value} handleFilter={handleFilter} />
          <DataGrid
            autoHeight
            rows={getdata}
            columns={columns}
            checkboxSelection
            pageSize={pageSize}
            disableSelectionOnClick
            rowsPerPageOptions={[10, 25, 50]}
            onPageSizeChange={newPageSize => setPageSize(newPageSize)}
            getRowId={(row) => row.StartDate}
          />
        </Card>
      </Grid>

      <AddUserDrawer />
    </Grid>
  )
}



export default WorkStructureDashboard 

///==============


// ** React Imports
// import { useEffect, useCallback, useState } from 'react'

// // ** Next Import
// import Link from 'next/link'

// // ** MUI Imports
// import Box from '@mui/material/Box'
// import Card from '@mui/material/Card'
// import Grid from '@mui/material/Grid'
// import { DataGrid } from '@mui/x-data-grid'
// import IconButton from '@mui/material/IconButton'
// import Typography from '@mui/material/Typography'

// // ** Icon Imports
// import Icon from 'src/@core/components/icon'

// // ** Store Imports
// import { useDispatch, useSelector } from 'react-redux'

// // ** Custom Components Imports
// import CustomChip from 'src/@core/components/mui/chip'
// import CustomAvatar from 'src/@core/components/mui/avatar'

// // ** Utils Import
// import { getInitials } from 'src/@core/utils/get-initials'

// // ** Actions Imports
// import { fetchData } from 'src/store/apps/user'

// // ** Custom Components Imports
// import TableHeader from 'src/views/apps/roles/TableHeader'

// //axios
// import axios from 'axios'

// function createData(ServiceId, UserId, PersonId) {
//   return { ServiceId, UserId, PersonId };
// }


// // ** Vars
// const userRoleObj = {
//   admin: { icon: 'mdi:laptop', color: 'error.main' },
//   author: { icon: 'mdi:cog-outline', color: 'warning.main' },
//   editor: { icon: 'mdi:pencil-outline', color: 'info.main' },
//   maintainer: { icon: 'mdi:chart-donut', color: 'success.main' },
//   subscriber: { icon: 'mdi:account-outline', color: 'primary.main' }
// }

// const userStatusObj = {
//   active: 'success',
//   pending: 'warning',
//   inactive: 'secondary'
// }

// // ** renders client column
// const renderClient = row => {

// }



// const columns = [

//   {
//     flex: 0.2,
//     minWidth: 230,
//     field: 'ServiceId',
//     headerName: 'serviceId',
//     renderCell: ({ row }) => {
//       const { fullName, username } = row

//       return (
//         <Box sx={{ display: 'flex', alignItems: 'center' }}>
//           {renderClient(row)}
//           <Box sx={{ display: 'flex', alignItems: 'flex-start', flexDirection: 'column' }}>
//             <Typography
//               noWrap
//               variant='body2'
//               component={Link}
//               href='/apps/user/view/overview/'
//               sx={{
//                 fontWeight: 600,
//                 color: 'text.primary',
//                 textDecoration: 'none',
//                 '&:hover': { color: theme => theme.palette.primary.main }
//               }}
//             >
              
//             </Typography>
//             <Typography noWrap variant='caption'>
//             {row.ServiceId}
//             </Typography>
//           </Box>
//         </Box>
//       )
//     }
//   },

//   {
//     flex: 0.15,
//     field: 'UserId',
//     minWidth: 150,
//     headerName: 'userId',
//     renderCell: ({ row }) => {
//       return (
//         <Box style={{ display: 'flex', alignItems: 'center' }}>
//           <Icon fontSize={20} />
//           <Typography noWrap sx={{ color: 'text.secondary', textTransform: 'capitalize' }}>
//             {row.UserId}
//           </Typography>
//         </Box>
//       )
//     }
//   },

//   {
//     flex: 0.15,
//     minWidth: 120,
//     headerName: 'personId',
//     field: 'PersonId',
//     renderCell: ({ row }) => {
//       return (
//         <Typography noWrap sx={{ textTransform: 'capitalize' }}>
//           {row.PersonId}
//         </Typography>
//       )
//     }
//   },

//   // {
//   //   flex: 0.1,
//   //   minWidth: 110,
//   //   field: 'email',
//   //   headerName: 'Email Id',
//   //   renderCell: ({ row }) => {
//   //     return (
//   //       <CustomChip
//   //         skin='light'
//   //         size='small'
//   //         label={row.email}
//   //         color={userStatusObj[row.email]}
//   //         sx={{ textTransform: 'capitalize' }}
//   //       />
//   //     )
//   //   }
//   // },

//   // {
//   //   flex: 0.1,
//   //   minWidth: 100,
//   //   sortable: false,
//   //   field: 'address',
//   //   headerName: 'Address',
//   //   renderCell: (row) => (
//   //     <IconButton component={Link} href='/apps/user/view/overview/'>
//   //       <Icon icon='mdi:eye-outline' />
//   //     </IconButton>
//   //   )
//   // }


// ]

// const UserList = () => {
//   // ** State
//   const [plan, setPlan] = useState('')
//   const [value, setValue] = useState('')
//   const [pageSize, setPageSize] = useState(10)

//   // Api Intregration by using Get method
//    const [getdata, setGetdata] = useState([]) 
//   const viewData = async () => {
//     let response = await axios.get(`https://webapidev.aitalkx.com/chr/leave/ReadLeaveDetailData?userId=45bba746-3309-49b7-9c03-b5793369d73c`)
//     setGetdata(response.data)
//     //console.log(response.data, "response data")
//   }
  
//   useEffect(() => {
//     viewData()
//   }, [])
  

//   // ** Hooks
//   // const dispatch = useDispatch()
//   // const store = useSelector(state => state.user)
//   // useEffect(() => {
//   //   dispatch(
//   //     fetchData({
//   //       role: '',
//   //       q: value,
//   //       status: '',
//   //       currentPlan: plan
//   //     })
//   //   )
//   // }, [dispatch, plan, value])

//   const handleFilter = useCallback(val => {
//     setValue(val)
//   }, [])

//   const handlePlanChange = useCallback(e => {
//     setPlan(e.target.value)
//   }, [])
//   console.log("getdata",getdata)

//   return (
//     <Grid container spacing={6}>
//       <Grid item xs={12}>
//         <Card>
//           <TableHeader plan={plan} value={value} handleFilter={handleFilter} handlePlanChange= {handlePlanChange} />
//                 <DataGrid
//                   autoHeight
//                   rows={getdata}
//                   columns={columns}
//                   checkboxSelection
//                   pageSize={pageSize}
//                   disableSelectionOnClick
//                   rowsPerPageOptions={[10, 25, 50]}
//                   onPageSizeChange={newPageSize => setPageSize(newPageSize)}
//                   getRowId={(row) =>  row.ServiceId}
//                 />
//         </Card>
//       </Grid>
//     </Grid>
//   )
// }  

// export default UserList
