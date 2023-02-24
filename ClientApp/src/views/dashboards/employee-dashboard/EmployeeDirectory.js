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
  // AssignmentId,
  // ActiveTab,
  // DataAction,
  // UserRoleCodes,
  // PersonId,
  EmployeeId,
  // NoteId,
  // NoteContractId,
  // NoteAssignmentId,
  // NotePositionHierarchyId,
  // NoteSalaryInfoId,
  // NoteLocationId,
  // NoteGradeId,
  // NotePositionId,
  // NoteDepartmentId,
  // NoteJobId,
  // ContractId,
  // PositionHierarchyId,
  // HierarchyId,
  // SalaryInfoId,
  // EmployeeContractNoteId,
  // ClosingBalance,
  PersonFullName,
  // IqamahNo,
  // PersonStatus,
  // AssignmentTypeCode,
  // AssignmentTypeName,
  // AssignmentStatusId,
  // AssignmentStatusName,
  // IsPrimaryAssignment,
  // DateOfJoin,
  // DTDateOfJoin,
  // AssignmentMasterDateOfJoin,
  // ProbationPeriod,
  // ProbationPeriodName,
  // ProbationEndDate,
  // NoticeStartDate,
  // NoticePeriod,
  // ActualTerminationDate,
  // LocationId,
  // LocationName,
  // GradeId,
  // GradeName,
  // JobGrade,
  // PhotoId,
  // UserId,
  // PhotoVersion,
  // PhotoName,
  // Page,
  // DepartmentId,
   DepartmentName,
  // JobId,
  JobName,
  // PositionId,
  // PositionName,
  // PositionTitle,
  // SupervisorId,
  // SupervisorName,
  // Remarks,
  // PayAnnualTicketForSelf,
  // PayAnnualTicketForDependent,
  // TicketDestinationId,
  // TicketDestination,
  // ClassOfTravelId,
  // DisableControl,
  // SectionId,
  // SectionName,
  // LegalEntityCode,
  Email,
  // WorkPhone,
  // WorkPhoneExtension,
  // JDNoteId,
  // PersonNo,
  // SponsorshipNo,
  // SponsorName,
  // EnableRemoteSignIn,
  // UserStatus,
  // ChangedData,
  // EffectiveDate,
  // OldValue,
  // NewValue,
  // ChangeStatus,
  // BasicSalary,
  // AnnualLeaveEntitlement,
  // PerformanceDocumentId,
  // PerformanceDocumentName,
  // PDStartDate,
  // PDEndDate,
  // PDStatus,
  // PDYear,
  // PDFinalRatingRounded,
  // PDBonus,
  // PDIncrement,
  // PDStageName,
  // PDStage,
  // Goal,
  // Competency,
  // ManagerJobName,
  // ManagerPersonFullName,
  // ManagerUserId,
  // EffectiveStartDate,
  // EffectiveEndDate,
  // BadgeAwardDate,
  // AssessmentName,
  // AssessmentScore,
  // AssessmentStartTime,
  // Criterias,
  // CriteriasList,
  // BadgeName,
  // BadgeDescription,
  // BadgeImage,
  // CertificationName,
  // CertificateReferenceNo,
  // ExpiryLicenseDat,
  // ResultScore,
  // CompetencyName,
  // CurrentDateText,
  // SponsorLogoId,
  // SponsorLogo,
  // FinalScore,
  // FinalComments,
  // AssignmentGradeId,
  // AssignmentTypeId,
  // OrgLevel1Id,
  // OrgLevel2Id,
  // OrgLevel3Id,
  // Org4Id,
  // BrandId,
  // MarketId,
  // ProvinceId,
  // CareerLevelId,
  // ContractType,
  // ContractRenewable,
  // NtsNoteId,
  Mobile,
  // ManagerPhoto,
  // LineManager,
  // FinalScoreImage,
  // Id,
  // CreatedDate,
  // CreatedBy,
  // LastUpdatedDate,
  // LastUpdatedBy,
  // IsDeleted,
  // SequenceOrder,
  // CompanyId,
  // Status,
  // VersionNo
) {
return { 
  // AssignmentId,
  // ActiveTab,
  // DataAction,
  // UserRoleCodes,
  // PersonId,
  EmployeeId,
  // NoteId,
  // NoteContractId,
  // NoteAssignmentId,
  // NotePositionHierarchyId,
  // NoteSalaryInfoId,
  // NoteLocationId,
  // NoteGradeId,
  // NotePositionId,
  // NoteDepartmentId,
  // NoteJobId,
  // ContractId,
  // PositionHierarchyId,
  // HierarchyId,
  // SalaryInfoId,
  // EmployeeContractNoteId,
  // ClosingBalance,
  PersonFullName,
  // IqamahNo,
  // PersonStatus,
  // AssignmentTypeCode,
  // AssignmentTypeName,
  // AssignmentStatusId,
  // AssignmentStatusName,
  // IsPrimaryAssignment,
  // DateOfJoin,
  // DTDateOfJoin,
  // AssignmentMasterDateOfJoin,
  // ProbationPeriod,
  // ProbationPeriodName,
  // ProbationEndDate,
  // NoticeStartDate,
  // NoticePeriod,
  // ActualTerminationDate,
  // LocationId,
  // LocationName,
  // GradeId,
  // GradeName,
  // JobGrade,
  // PhotoId,
  // UserId,
  // PhotoVersion,
  // PhotoName,
  // Page,
  // DepartmentId,
  DepartmentName,
  // JobId,
  JobName,
  // PositionId,
  // PositionName,
  // PositionTitle,
  // SupervisorId,
  // SupervisorName,
  // Remarks,
  // PayAnnualTicketForSelf,
  // PayAnnualTicketForDependent,
  // TicketDestinationId,
  // TicketDestination,
  // ClassOfTravelId,
  // DisableControl,
  // SectionId,
  // SectionName,
  // LegalEntityCode,
  Email,
  // WorkPhone,
  // WorkPhoneExtension,
  // JDNoteId,
  // PersonNo,
  // SponsorshipNo,
  // SponsorName,
  // EnableRemoteSignIn,
  // UserStatus,
  // ChangedData,
  // EffectiveDate,
  // OldValue,
  // NewValue,
  // ChangeStatus,
  // BasicSalary,
  // AnnualLeaveEntitlement,
  // PerformanceDocumentId,
  // PerformanceDocumentName,
  // PDStartDate,
  // PDEndDate,
  // PDStatus,
  // PDYear,
  // PDFinalRatingRounded,
  // PDBonus,
  // PDIncrement,
  // PDStageName,
  // PDStage,
  // Goal,
  // Competency,
  // ManagerJobName,
  // ManagerPersonFullName,
  // ManagerUserId,
  // EffectiveStartDate,
  // EffectiveEndDate,
  // BadgeAwardDate,
  // AssessmentName,
  // AssessmentScore,
  // AssessmentStartTime,
  // Criterias,
  // CriteriasList,
  // BadgeName,
  // BadgeDescription,
  // BadgeImage,
  // CertificationName,
  // CertificateReferenceNo,
  // ExpiryLicenseDat,
  // ResultScore,
  // CompetencyName,
  // CurrentDateText,
  // SponsorLogoId,
  // SponsorLogo,
  // FinalScore,
  // FinalComments,
  // AssignmentGradeId,
  // AssignmentTypeId,
  // OrgLevel1Id,
  // OrgLevel2Id,
  // OrgLevel3Id,
  // Org4Id,
  // BrandId,
  // MarketId,
  // ProvinceId,
  // CareerLevelId,
  // ContractType,
  // ContractRenewable,
  // NtsNoteId,
  Mobile,
  // ManagerPhoto,
  // LineManager,
  // FinalScoreImage,
  // Id,
  // CreatedDate,
  // CreatedBy,
  // LastUpdatedDate,
  // LastUpdatedBy,
  // IsDeleted,
  // SequenceOrder,
  // CompanyId,
  // Status,
  // VersionNo
};
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
  // {
  //   flex: 0.2,
  //   minWidth: 250,
  //   field: 'AssignmentId',
  //   headerName: 'Assignment Id',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Typography noWrap variant='body2'>
  //         {row.AssignmentId}
  //       </Typography>
  //     )
  //   }
  // },
  // {
  //   flex: 0.2,
  //   minWidth: 250,
  //   field: 'ActiveTab',
  //   headerName: 'Active Tab',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Typography noWrap variant='body2'>
  //         {row.ActiveTab}
  //       </Typography>
  //     )
  //   }
  // },
  // {
  //   flex: 0.15,
  //   field: 'DataAction',
  //   minWidth: 150,
  //   headerName: 'Data Action',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Box sx={{ display: 'flex', alignItems: 'center'}}>
  //         <Icon fontSize={20} />
  //         <Typography noWrap sx={{ color: 'text.secondary', textTransform: 'capitalize' }}>
  //           {row.DataAction}
  //         </Typography>
  //       </Box>
  //     )
  //   }
  // },
  // {
  //   flex: 0.15,
  //   minWidth: 120,
  //   headerName: ' User Role Codes',
  //   field: ' UserRoleCodes',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Typography noWrap sx={{ textTransform: 'capitalize' }}>
  //         {row.UserRoleCodes}
  //       </Typography>
  //     )
  //   }
  // },
  // {
  //   flex: 0.15,
  //   minWidth: 120,
  //   headerName: 'Person Id',
  //   field: 'PersonId',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Typography noWrap sx={{ textTransform: 'capitalize' }}>
  //         {row.PersonId}
  //       </Typography>
  //     )
  //   }
  // },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'EmployeeId',
    headerName: 'Employee Id',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.EmployeeId}
        </Typography>
      )
    }
  },
  // {
  //   flex: 0.2,
  //   minWidth: 250,
  //   field: 'NoteId',
  //   headerName: 'Note Id',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Typography noWrap variant='body2'>
  //         {row.NoteId}
  //       </Typography>
  //     )
  //   }
  // },
  // {
  //   flex: 0.15,
  //   field: 'NoteContractId',
  //   minWidth: 150,
  //   headerName: 'Note Contract Id',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Box sx={{ display: 'flex', alignItems: 'center'}}>
  //         <Icon fontSize={20} />
  //         <Typography noWrap sx={{ color: 'text.secondary', textTransform: 'capitalize' }}>
  //           {row.NoteContractId}
  //         </Typography>
  //       </Box>
  //     )
  //   }
  // },
  // {
  //   flex: 0.15,
  //   minWidth: 120,
  //   headerName: 'Note Assignment Id',
  //   field: 'NoteAssignmentId',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Typography noWrap sx={{ textTransform: 'capitalize' }}>
  //         {row.NoteAssignmentId}
  //       </Typography>
  //     )
  //   }
  // },
  // {
  //   flex: 0.2,
  //   minWidth: 250,
  //   field: 'NotePositionHierarchyId',
  //   headerName: 'Note Position Hierarchy Id',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Typography noWrap variant='body2'>
  //         {row.NotePositionHierarchyId}
  //       </Typography>
  //     )
  //   }
  // },

  // {
  //   flex: 0.2,
  //   minWidth: 250,
  //   field: 'NoteSalaryInfoId',
  //   headerName: 'Note Salary Info Id',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Typography noWrap variant='body2'>
  //         {row.NoteSalaryInfoId}
  //       </Typography>
  //     )
  //   }
  // },
  // {
  //   flex: 0.15,
  //   field: 'NoteLocationId',
  //   minWidth: 150,
  //   headerName: 'Note Location Id',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Box sx={{ display: 'flex', alignItems: 'center'}}>
  //         <Icon fontSize={20} />
  //         <Typography noWrap sx={{ color: 'text.secondary', textTransform: 'capitalize' }}>
  //           {row.NoteLocationId}
  //         </Typography>
  //       </Box>
  //     )
  //   }
  // },
  // {
  //   flex: 0.15,
  //   minWidth: 120,
  //   headerName: 'Note Grade Id',
  //   field: 'NoteGradeId',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Typography noWrap sx={{ textTransform: 'capitalize' }}>
  //         {row.NoteGradeId}
  //       </Typography>
  //     )
  //   }
  // },
  // {
  //   flex: 0.15,
  //   minWidth: 120,
  //   headerName: 'Note Position Id',
  //   field: 'NotePositionId',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Typography noWrap sx={{ textTransform: 'capitalize' }}>
  //         {row.NotePositionId}
  //       </Typography>
  //     )
  //   }
  // },
  // {
  //   flex: 0.2,
  //   minWidth: 250,
  //   field: 'NoteDepartmentId',
  //   headerName: 'Note Department Id',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Typography noWrap variant='body2'>
  //         {row.NoteDepartmentId}
  //       </Typography>
  //     )
  //   }
  // },
  // {
  //   flex: 0.15,
  //   field: 'NoteJobId',
  //   minWidth: 150,
  //   headerName: 'Note Job Id',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Box sx={{ display: 'flex', alignItems: 'center'}}>
  //         <Icon fontSize={20} />
  //         <Typography noWrap sx={{ color: 'text.secondary', textTransform: 'capitalize' }}>
  //           {row.NoteJobId}
  //         </Typography>
  //       </Box>
  //     )
  //   }
  // },
  // {
  //   flex: 0.15,
  //   minWidth: 120,
  //   headerName: 'Contract Id',
  //   field: 'ContractId',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Typography noWrap sx={{ textTransform: 'capitalize' }}>
  //         {row.ContractId}
  //       </Typography>
  //     )
  //   }
  // },
  // {
  //   flex: 0.15,
  //   minWidth: 120,
  //   headerName: 'Position Hierarchy Id',
  //   field: 'PositionHierarchyId',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Typography noWrap sx={{ textTransform: 'capitalize' }}>
  //         {row.PositionHierarchyId}
  //       </Typography>
  //     )
  //   }
  // },
  // {
  //   flex: 0.2,
  //   minWidth: 250,
  //   field: 'HierarchyId',
  //   headerName: 'Hierarchy Id',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Typography noWrap variant='body2'>
  //         {row.HierarchyId}
  //       </Typography>
  //     )
  //   }
  // },
  // {
  //   flex: 0.2,
  //   minWidth: 250,
  //   field: 'SalaryInfoId',
  //   headerName: 'Salary Info Id',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Typography noWrap variant='body2'>
  //         {row.SalaryInfoId}
  //       </Typography>
  //     )
  //   }
  // },

  // {
  //   flex: 0.2,
  //   minWidth: 250,
  //   field: 'EmployeeContractNoteId',
  //   headerName: 'Employee Contract Note Id',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Typography noWrap variant='body2'>
  //         {row.EmployeeContractNoteId}
  //       </Typography>
  //     )
  //   }
  // },
  // {
  //   flex: 0.2,
  //   minWidth: 250,
  //   field: 'ClosingBalance',
  //   headerName: 'Closing Balance',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Typography noWrap variant='body2'>
  //         {row.ClosingBalance}
  //       </Typography>
  //     )
  //   }
  // },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'PersonFullName',
    headerName: ' Employee Full Name',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.PersonFullName}
        </Typography>
      )
    }
  },
  // {
  //   flex: 0.2,
  //   minWidth: 250,
  //   field: 'IqamahNo',
  //   headerName: 'Iqamah No',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Typography noWrap variant='body2'>
  //         {row.IqamahNo}
  //       </Typography>
  //     )
  //   }
  // },
  // {
  //   flex: 0.2,
  //   minWidth: 250,
  //   field: 'PersonStatus',
  //   headerName: 'Person Status',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Typography noWrap variant='body2'>
  //         {row.PersonStatus}
  //       </Typography>
  //     )
  //   }
  // },
  // {
  //   flex: 0.2,
  //   minWidth: 250,
  //   field: 'AssignmentTypeCode',
  //   headerName: 'Assignment Type Code',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Typography noWrap variant='body2'>
  //         {row.AssignmentTypeCode}
  //       </Typography>
  //     )
  //   }
  // },
  // {
  //   flex: 0.2,
  //   minWidth: 250,
  //   field: 'AssignmentTypeName',
  //   headerName: 'Assignment Type Name',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Typography noWrap variant='body2'>
  //         {row.AssignmentTypeName}
  //       </Typography>
  //     )
  //   }
  // },
  // {
  //   flex: 0.2,
  //   minWidth: 250,
  //   field: 'AssignmentStatusId',
  //   headerName: 'Assignment Status Id',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Typography noWrap variant='body2'>
  //         {row.AssignmentStatusId}
  //       </Typography>
  //     )
  //   }
  // },
  // {
  //   flex: 0.2,
  //   minWidth: 250,
  //   field: 'AssignmentStatusName',
  //   headerName: 'Assignment Status Name',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Typography noWrap variant='body2'>
  //         {row.AssignmentStatusName}
  //       </Typography>
  //     )
  //   }
  // },
  // {
  //   flex: 0.2,
  //   minWidth: 250,
  //   field: ' IsPrimaryAssignment',
  //   headerName: ' Is Primary Assignment',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Typography noWrap variant='body2'>
  //         {row.IsPrimaryAssignment}
  //       </Typography>
  //     )
  //   }
  // },

  // {
  //   flex: 0.2,
  //   minWidth: 250,
  //   field: 'DateOfJoin',
  //   headerName: 'Date Of Join',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Typography noWrap variant='body2'>
  //         {row.DateOfJoin}
  //       </Typography>
  //     )
  //   }
  // },
  // {
  //   flex: 0.2,
  //   minWidth: 250,
  //   field: 'DTDateOfJoin',
  //   headerName: 'DTDate Of Join',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Typography noWrap variant='body2'>
  //         {row.DTDateOfJoin}
  //       </Typography>
  //     )
  //   }
  // },
  // {
  //   flex: 0.2,
  //   minWidth: 250,
  //   field: 'AssignmentMasterDateOfJoin',
  //   headerName: 'Assignment Master Date Of Join',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Typography noWrap variant='body2'>
  //         {row.AssignmentMasterDateOfJoin}
  //       </Typography>
  //     )
  //   }
  // },
  // {
  //   flex: 0.2,
  //   minWidth: 250,
  //   field: 'ProbationPeriod',
  //   headerName: 'Probation Period',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Typography noWrap variant='body2'>
  //         {row.ProbationPeriod}
  //       </Typography>
  //     )
  //   }
  // },
  // {
  //   flex: 0.2,
  //   minWidth: 250,
  //   field: 'ProbationPeriodName',
  //   headerName: 'Probation Period Name',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Typography noWrap variant='body2'>
  //         {row.ProbationPeriodName}
  //       </Typography>
  //     )
  //   }
  // },
  // {
  //   flex: 0.2,
  //   minWidth: 250,
  //   field: 'ProbationEndDate',
  //   headerName: 'Probation End Date',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Typography noWrap variant='body2'>
  //         {row.ProbationEndDate}
  //       </Typography>
  //     )
  //   }
  // },
  // {
  //   flex: 0.2,
  //   minWidth: 250,
  //   field: 'NoticeStartDate',
  //   headerName: 'Notice Start Date',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Typography noWrap variant='body2'>
  //         {row.NoticeStartDate}
  //       </Typography>
  //     )
  //   }
  // },
  // {
  //   flex: 0.2,
  //   minWidth: 250,
  //   field: 'NoticePeriod',
  //   headerName: 'Notice Period',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Typography noWrap variant='body2'>
  //         {row.NoticePeriod}
  //       </Typography>
  //     )
  //   }
  // },
  // {
  //   flex: 0.2,
  //   minWidth: 250,
  //   field: 'ActualTerminationDate',
  //   headerName: 'Actual Termination Date',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Typography noWrap variant='body2'>
  //         {row.ActualTerminationDate}
  //       </Typography>
  //     )
  //   }
  // },
  // {
  //   flex: 0.2,
  //   minWidth: 250,
  //   field: 'LocationId',
  //   headerName: 'Location Id',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Typography noWrap variant='body2'>
  //         {row.LocationId}
  //       </Typography>
  //     )
  //   }
  // },

  // {
  //   flex: 0.2,
  //   minWidth: 250,
  //   field: 'LocationName',
  //   headerName: 'Location Name',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Typography noWrap variant='body2'>
  //         {row.LocationName}
  //       </Typography>
  //     )
  //   }
  // },
  // {
  //   flex: 0.2,
  //   minWidth: 250,
  //   field: 'GradeId',
  //   headerName: 'Grade Id',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Typography noWrap variant='body2'>
  //         {row.GradeId}
  //       </Typography>
  //     )
  //   }
  // },
  // {
  //   flex: 0.2,
  //   minWidth: 250,
  //   field: 'GradeName',
  //   headerName: 'Grade Name',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Typography noWrap variant='body2'>
  //         {row.GradeName}
  //       </Typography>
  //     )
  //   }
  // },
  // {
  //   flex: 0.2,
  //   minWidth: 250,
  //   field: 'JobGrade',
  //   headerName: 'Job Grade',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Typography noWrap variant='body2'>
  //         {row.JobGrade}
  //       </Typography>
  //     )
  //   }
  // },
  // {
  //   flex: 0.2,
  //   minWidth: 250,
  //   field: 'PhotoId',
  //   headerName: 'Photo Id',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Typography noWrap variant='body2'>
  //         {row.PhotoId}
  //       </Typography>
  //     )
  //   }
  // },
  // {
  //   flex: 0.2,
  //   minWidth: 250,
  //   field: 'UserId',
  //   headerName: 'User Id',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Typography noWrap variant='body2'>
  //         {row.UserId}
  //       </Typography>
  //     )
  //   }
  // },
  // {
  //   flex: 0.2,
  //   minWidth: 250,
  //   field: 'PhotoVersion',
  //   headerName: 'Photo Version',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Typography noWrap variant='body2'>
  //         {row.PhotoVersion}
  //       </Typography>
  //     )
  //   }
  // },
  // {
  //   flex: 0.2,
  //   minWidth: 250,
  //   field: 'PhotoName',
  //   headerName: 'Photo Name',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Typography noWrap variant='body2'>
  //         {row.PhotoName}
  //       </Typography>
  //     )
  //   }
  // },
  // {
  //   flex: 0.2,
  //   minWidth: 250,
  //   field: 'Page',
  //   headerName: 'Page',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Typography noWrap variant='body2'>
  //         {row.Page}
  //       </Typography>
  //     )
  //   }
  // },
  // {
  //   flex: 0.2,
  //   minWidth: 250,
  //   field: 'DepartmentId',
  //   headerName: 'Department Id',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Typography noWrap variant='body2'>
  //         {row.DepartmentId}
  //       </Typography>
  //     )
  //   }
  // },

  {
    flex: 0.2,
    minWidth: 250,
    field: 'DepartmentName',
    headerName: 'Department Name',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.DepartmentName}
        </Typography>
      )
    }
  },
  // {
  //   flex: 0.2,
  //   minWidth: 250,
  //   field: 'JobId',
  //   headerName: 'Job Id',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Typography noWrap variant='body2'>
  //         {row.JobId}
  //       </Typography>
  //     )
  //   }
  // },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'JobName',
    headerName: 'Job Title',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.JobName}
        </Typography>
      )
    }
  },
  // {
  //   flex: 0.2,
  //   minWidth: 250,
  //   field: 'PositionId',
  //   headerName: 'Position Id',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Typography noWrap variant='body2'>
  //         {row.PositionId}
  //       </Typography>
  //     )
  //   }
  // },
  // {
  //   flex: 0.2,
  //   minWidth: 250,
  //   field: 'PositionName',
  //   headerName: 'Position Name',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Typography noWrap variant='body2'>
  //         {row.PositionName}
  //       </Typography>
  //     )
  //   }
  // },
  // {
  //   flex: 0.2,
  //   minWidth: 250,
  //   field: 'PositionTitle',
  //   headerName: 'Position Title',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Typography noWrap variant='body2'>
  //         {row.PositionTitle}
  //       </Typography>
  //     )
  //   }
  // },
  // {
  //   flex: 0.2,
  //   minWidth: 250,
  //   field: 'SupervisorId ',
  //   headerName: 'Supervisor Id ',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Typography noWrap variant='body2'>
  //         {row.SupervisorId }
  //       </Typography>
  //     )
  //   }
  // },
  // {
  //   flex: 0.2,
  //   minWidth: 250,
  //   field: 'SupervisorName',
  //   headerName: 'Supervisor Name',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Typography noWrap variant='body2'>
  //         {row.SupervisorName}
  //       </Typography>
  //     )
  //   }
  // },
  // {
  //   flex: 0.2,
  //   minWidth: 250,
  //   field: 'Remarks',
  //   headerName: 'Remarks',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Typography noWrap variant='body2'>
  //         {row.Remarks}
  //       </Typography>
  //     )
  //   }
  // },
  // {
  //   flex: 0.2,
  //   minWidth: 250,
  //  field: 'PayAnnualTicketForSelf',
  //  headerName: 'PayAnnual Ticket For Self',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Typography noWrap variant='body2'>
  //         {row.PayAnnualTicketForSelf}
  //       </Typography>
  //     )
  //   }
  // },

  // {
  //   flex: 0.2,
  //   minWidth: 250,
  //   field: 'PayAnnualTicketForDependent',
  //   headerName: 'PayAnnual Ticket For Dependent',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Typography noWrap variant='body2'>
  //         {row.PayAnnualTicketForDependent}
  //       </Typography>
  //     )
  //   }
  // },
  // {
  //   flex: 0.2,
  //   minWidth: 250,
  //   field: 'TicketDestinationId',
  //   headerName: 'Ticket Destination Id',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Typography noWrap variant='body2'>
  //         {row.TicketDestinationId}
  //       </Typography>
  //     )
  //   }
  // },
  // {
  //   flex: 0.2,
  //   minWidth: 250,
  //   field: 'TicketDestination',
  //   headerName: 'Ticket Destination',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Typography noWrap variant='body2'>
  //         {row.TicketDestination}
  //       </Typography>
  //     )
  //   }
  // },
  // {
  //   flex: 0.2,
  //   minWidth: 250,
  //   field: 'ClassOfTravelId',
  //   headerName: 'Class Of Travel Id',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Typography noWrap variant='body2'>
  //         {row.ClassOfTravelId}
  //       </Typography>
  //     )
  //   }
  // },
  // {
  //   flex: 0.2,
  //   minWidth: 250,
  //   field: 'DisableControl',
  //   headerName: 'Disable Control',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Typography noWrap variant='body2'>
  //         {row.DisableControl}
  //       </Typography>
  //     )
  //   }
  // },
  // {
  //   flex: 0.2,
  //   minWidth: 250,
  //   field: 'SectionId',
  //   headerName: 'Section Id',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Typography noWrap variant='body2'>
  //         {row.SectionId}
  //       </Typography>
  //     )
  //   }
  // },
  // {
  //   flex: 0.2,
  //   minWidth: 250,
  //   field: 'SectionName',
  //   headerName: 'Section Name',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Typography noWrap variant='body2'>
  //         {row.SectionName}
  //       </Typography>
  //     )
  //   }
  // },
  // {
  //   flex: 0.2,
  //   minWidth: 250,
  //   field: 'LegalEntityCode',
  //   headerName: 'Legal Entity Code',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Typography noWrap variant='body2'>
  //         {row.LegalEntityCode}
  //       </Typography>
  //     )
  //   }
  // },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'Email',
    headerName: 'Email',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.Email}
        </Typography>
      )
    }
  },
  // {
  //   flex: 0.2,
  //   minWidth: 250,
  //   field: 'WorkPhone',
  //   headerName: ' Work Phone',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Typography noWrap variant='body2'>
  //         {row.WorkPhone}
  //       </Typography>
  //     )
  //   }
  // },

  // {
  //   flex: 0.2,
  //   minWidth: 250,
  //   field: 'WorkPhoneExtension',
  //   headerName: 'Work Phone Extension',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Typography noWrap variant='body2'>
  //         {row.WorkPhoneExtension}
  //       </Typography>
  //     )
  //   }
  // },
  // {
  //   flex: 0.2,
  //   minWidth: 250,
  //   field: 'JDNoteId',
  //   headerName: 'JD Note Id',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Typography noWrap variant='body2'>
  //         {row.JDNoteId}
  //       </Typography>
  //     )
  //   }
  // },
  // {
  //   flex: 0.2,
  //   minWidth: 250,
  //   field: 'PersonNo',
  //   headerName: 'Person No',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Typography noWrap variant='body2'>
  //         {row.PersonNo}
  //       </Typography>
  //     )
  //   }
  // },
  // {
  //   flex: 0.2,
  //   minWidth: 250,
  //   field: 'SponsorshipNo',
  //   headerName: 'Sponsorship No',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Typography noWrap variant='body2'>
  //         {row.SponsorshipNo}
  //       </Typography>
  //     )
  //   }
  // },
  // {
  //   flex: 0.2,
  //   minWidth: 250,
  //   field: 'SponsorName',
  //   headerName: 'Sponsor Name',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Typography noWrap variant='body2'>
  //         {row.SponsorName}
  //       </Typography>
  //     )
  //   }
  // },
  // {
  //   flex: 0.2,
  //   minWidth: 250,
  //   field: 'EnableRemoteSignIn',
  //   headerName: ' Enable Remote Sign In',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Typography noWrap variant='body2'>
  //         {row.EnableRemoteSignIn}
  //       </Typography>
  //     )
  //   }
  // },
  // {
  //   flex: 0.2,
  //   minWidth: 250,
  //   field: 'UserStatus',
  //   headerName: 'User Status',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Typography noWrap variant='body2'>
  //         {row.UserStatus}
  //       </Typography>
  //     )
  //   }
  // },
  // {
  //   flex: 0.2,
  //   minWidth: 250,
  //   field: 'ChangedData',
  //   headerName: 'Changed Data',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Typography noWrap variant='body2'>
  //         {row.ChangedData}
  //       </Typography>
  //     )
  //   }
  // },
  // {
  //   flex: 0.2,
  //   minWidth: 250,
  //   field: 'EffectiveDate',
  //   headerName: 'Effective Date',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Typography noWrap variant='body2'>
  //         {row.EffectiveDate}
  //       </Typography>
  //     )
  //   }
  // },
  // {
  //   flex: 0.2,
  //   minWidth: 250,
  //   field: 'OldValue',
  //   headerName: 'Old Value',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Typography noWrap variant='body2'>
  //         {row.OldValue}
  //       </Typography>
  //     )
  //   }
  // },

  // {
  //   flex: 0.2,
  //   minWidth: 250,
  //   field: 'NewValue',
  //   headerName: 'New Value',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Typography noWrap variant='body2'>
  //         {row.NewValue}
  //       </Typography>
  //     )
  //   }
  // },
  // {
  //   flex: 0.2,
  //   minWidth: 250,
  //   field: 'ChangeStatus',
  //   headerName: 'Change Status',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Typography noWrap variant='body2'>
  //         {row.ChangeStatus}
  //       </Typography>
  //     )
  //   }
  // },
  // {
  //   flex: 0.2,
  //   minWidth: 250,
  //   field: 'BasicSalary',
  //   headerName: 'Basic Salary',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Typography noWrap variant='body2'>
  //         {row.BasicSalary}
  //       </Typography>
  //     )
  //   }
  // },
  // {
  //   flex: 0.2,
  //   minWidth: 250,
  //   field: 'AnnualLeaveEntitlement',
  //   headerName: 'Annual Leave Entitlement',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Typography noWrap variant='body2'>
  //         {row.AnnualLeaveEntitlement}
  //       </Typography>
  //     )
  //   }
  // },
  // {
  //   flex: 0.2,
  //   minWidth: 250,
  //   field: 'PerformanceDocumentId',
  //   headerName: 'Performance Document Id',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Typography noWrap variant='body2'>
  //         {row.PerformanceDocumentId}
  //       </Typography>
  //     )
  //   }
  // },
  // {
  //   flex: 0.2,
  //   minWidth: 250,
  //   field: 'PerformanceDocumentName',
  //   headerName: 'Performance Document Name',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Typography noWrap variant='body2'>
  //         {row.PerformanceDocumentName}
  //       </Typography>
  //     )
  //   }
  // },
  // {
  //   flex: 0.2,
  //   minWidth: 250,
  //   field: 'PDStartDate',
  //   headerName: 'PD Start Date',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Typography noWrap variant='body2'>
  //         {row.PDStartDate}
  //       </Typography>
  //     )
  //   }
  // },
  // {
  //   flex: 0.2,
  //   minWidth: 250,
  //   field: 'PDEndDate',
  //   headerName: 'PD End Date',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Typography noWrap variant='body2'>
  //         {row.PDEndDate}
  //       </Typography>
  //     )
  //   }
  // },
  // {
  //   flex: 0.2,
  //   minWidth: 250,
  //   field: 'PDStatus',
  //   headerName: 'PD Status',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Typography noWrap variant='body2'>
  //         {row.PDStatus}
  //       </Typography>
  //     )
  //   }
  // },
  // {
  //   flex: 0.2,
  //   minWidth: 250,
  //   field: 'PDYear',
  //   headerName: 'PD Year',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Typography noWrap variant='body2'>
  //         {row.PDYear}
  //       </Typography>
  //     )
  //   }
  // },

  // {
  //   flex: 0.2,
  //   minWidth: 250,
  //   field: 'PDFinalRatingRounded',
  //   headerName: 'PDFinal Rating Rounded',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Typography noWrap variant='body2'>
  //         {row.PDFinalRatingRounded}
  //       </Typography>
  //     )
  //   }
  // },
  // {
  //   flex: 0.2,
  //   minWidth: 250,
  //   field: 'PDBonus',
  //   headerName: 'PD Bonus',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Typography noWrap variant='body2'>
  //         {row.PDBonus}
  //       </Typography>
  //     )
  //   }
  // },
  // {
  //   flex: 0.2,
  //   minWidth: 250,
  //   field: 'PDIncrement',
  //   headerName: 'PD Increment',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Typography noWrap variant='body2'>
  //         {row.PDIncrement}
  //       </Typography>
  //     )
  //   }
  // },
  // {
  //   flex: 0.2,
  //   minWidth: 250,
  //   field: 'PDStageName',
  //   headerName: 'PD Stage Name',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Typography noWrap variant='body2'>
  //         {row.PDStageName}
  //       </Typography>
  //     )
  //   }
  // },
  // {
  //   flex: 0.2,
  //   minWidth: 250,
  //   field: 'PDStage',
  //   headerName: 'PD Stage',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Typography noWrap variant='body2'>
  //         {row.PDStage}
  //       </Typography>
  //     )
  //   }
  // },
  // {
  //   flex: 0.2,
  //   minWidth: 250,
  //   field: 'Goal',
  //   headerName: 'Goal',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Typography noWrap variant='body2'>
  //         {row.Goal}
  //       </Typography>
  //     )
  //   }
  // },
  // {
  //   flex: 0.2,
  //   minWidth: 250,
  //   field: 'Competency',
  //   headerName: 'Competency',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Typography noWrap variant='body2'>
  //         {row.Competency}
  //       </Typography>
  //     )
  //   }
  // },
  // {
  //   flex: 0.2,
  //   minWidth: 250,
  //   field: 'ManagerJobName',
  //   headerName: 'Manager Job Name',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Typography noWrap variant='body2'>
  //         {row.ManagerJobName}
  //       </Typography>
  //     )
  //   }
  // },
  // {
  //   flex: 0.2,
  //   minWidth: 250,
  //   field: 'ManagerPersonFullName',
  //   headerName: 'Manager Person FullName',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Typography noWrap variant='body2'>
  //         {row.ManagerPersonFullName}
  //       </Typography>
  //     )
  //   }
  // },
  // {
  //   flex: 0.2,
  //   minWidth: 250,
  //   field: 'ManagerUserId',
  //   headerName: 'Manager User Id',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Typography noWrap variant='body2'>
  //         {row.ManagerUserId}
  //       </Typography>
  //     )
  //   }
  // },

  // {
  //   flex: 0.2,
  //   minWidth: 250,
  //   field: 'EffectiveStartDate',
  //   headerName: 'Effective Start Date',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Typography noWrap variant='body2'>
  //         {row.EffectiveStartDate}
  //       </Typography>
  //     )
  //   }
  // },
  // {
  //   flex: 0.2,
  //   minWidth: 250,
  //   field: 'EffectiveEndDate',
  //   headerName: 'Effective End Date',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Typography noWrap variant='body2'>
  //         {row.EffectiveEndDate}
  //       </Typography>
  //     )
  //   }
  // },
  // {
  //   flex: 0.2,
  //   minWidth: 250,
  //   field: 'BadgeAwardDate',
  //   headerName: 'Badge Award Date',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Typography noWrap variant='body2'>
  //         {row.BadgeAwardDate}
  //       </Typography>
  //     )
  //   }
  // },
  // {
  //   flex: 0.2,
  //   minWidth: 250,
  //   field: 'AssessmentName',
  //   headerName: 'Assessment Name',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Typography noWrap variant='body2'>
  //         {row.AssessmentName}
  //       </Typography>
  //     )
  //   }
  // },
  // {
  //   flex: 0.2,
  //   minWidth: 250,
  //   field: 'AssessmentScore',
  //   headerName: 'Assessment Score',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Typography noWrap variant='body2'>
  //         {row.AssessmentScore}
  //       </Typography>
  //     )
  //   }
  // },
  // {
  //   flex: 0.2,
  //   minWidth: 250,
  //   field: 'AssessmentStartTime',
  //   headerName: 'Assessment Start Time',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Typography noWrap variant='body2'>
  //         {row.AssessmentStartTime}
  //       </Typography>
  //     )
  //   }
  // },
  // {
  //   flex: 0.2,
  //   minWidth: 250,
  //   field: 'Criterias',
  //   headerName: 'Criterias',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Typography noWrap variant='body2'>
  //         {row.Criterias}
  //       </Typography>
  //     )
  //   }
  // },
  // {
  //   flex: 0.2,
  //   minWidth: 250,
  //   field: 'CriteriasList',
  //   headerName: 'CriteriasList',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Typography noWrap variant='body2'>
  //         {row.CriteriasList}
  //       </Typography>
  //     )
  //   }
  // },
  // {
  //   flex: 0.2,
  //   minWidth: 250,
  //   field: 'BadgeName',
  //   headerName: 'Badge Name',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Typography noWrap variant='body2'>
  //         {row.BadgeName}
  //       </Typography>
  //     )
  //   }
  // },
  // {
  //   flex: 0.2,
  //   minWidth: 250,
  //   field: 'BadgeDescription',
  //   headerName: 'Badge Description',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Typography noWrap variant='body2'>
  //         {row.BadgeDescription}
  //       </Typography>
  //     )
  //   }
  // },

  // {
  //   flex: 0.2,
  //   minWidth: 250,
  //   field: 'BadgeImage',
  //   headerName: 'Badge Image',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Typography noWrap variant='body2'>
  //         {row.BadgeImage}
  //       </Typography>
  //     )
  //   }
  // },
  // {
  //   flex: 0.2,
  //   minWidth: 250,
  //   field: 'CertificationName',
  //   headerName: 'Certification Name',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Typography noWrap variant='body2'>
  //         {row.CertificationName}
  //       </Typography>
  //     )
  //   }
  // },
  // {
  //   flex: 0.2,
  //   minWidth: 250,
  //   field: 'CertificateReferenceNo',
  //   headerName: 'Certificate Reference No',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Typography noWrap variant='body2'>
  //         {row.CertificateReferenceNo}
  //       </Typography>
  //     )
  //   }
  // },
  // {
  //   flex: 0.2,
  //   minWidth: 250,
  //   field: 'ExpiryLicenseDate',
  //   headerName: 'Expiry License Date',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Typography noWrap variant='body2'>
  //         {row.ExpiryLicenseDate}
  //       </Typography>
  //     )
  //   }
  // },
  // {
  //   flex: 0.2,
  //   minWidth: 250,
  //   field: 'ResultScore',
  //   headerName: 'Result Score',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Typography noWrap variant='body2'>
  //         {row.ResultScore}
  //       </Typography>
  //     )
  //   }
  // },
  // {
  //   flex: 0.2,
  //   minWidth: 250,
  //   field: 'CompetencyName',
  //   headerName: 'Competency Name',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Typography noWrap variant='body2'>
  //         {row.CompetencyName}
  //       </Typography>
  //     )
  //   }
  // },
  // {
  //   flex: 0.2,
  //   minWidth: 250,
  //   field: 'CurrentDateText',
  //   headerName: 'Current Date Text',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Typography noWrap variant='body2'>
  //         {row.CurrentDateText}
  //       </Typography>
  //     )
  //   }
  // },
  // {
  //   flex: 0.2,
  //   minWidth: 250,
  //   field: 'SponsorLogoId',
  //   headerName: 'Sponsor Logo Id',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Typography noWrap variant='body2'>
  //         {row.SponsorLogoId}
  //       </Typography>
  //     )
  //   }
  // },
  // {
  //   flex: 0.2,
  //   minWidth: 250,
  //   field: 'SponsorLogo',
  //   headerName: 'Sponsor Logo',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Typography noWrap variant='body2'>
  //         {row.SponsorLogo}
  //       </Typography>
  //     )
  //   }
  // },
  // {
  //   flex: 0.2,
  //   minWidth: 250,
  //   field: 'FinalScore',
  //   headerName: 'Final Score',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Typography noWrap variant='body2'>
  //         {row.FinalScore}
  //       </Typography>
  //     )
  //   }
  // },

  // {
  //   flex: 0.2,
  //   minWidth: 250,
  //   field: 'FinalComments',
  //   headerName: 'Final Comments',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Typography noWrap variant='body2'>
  //         {row.FinalComments}
  //       </Typography>
  //     )
  //   }
  // },
  // {
  //   flex: 0.2,
  //   minWidth: 250,
  //   field: 'AssignmentGradeId',
  //   headerName: 'Assignment Grade Id',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Typography noWrap variant='body2'>
  //         {row.AssignmentGradeId}
  //       </Typography>
  //     )
  //   }
  // },
  // {
  //   flex: 0.2,
  //   minWidth: 250,
  //   field: 'AssignmentTypeId',
  //   headerName: 'Assignment Type Id',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Typography noWrap variant='body2'>
  //         {row.AssignmentTypeId}
  //       </Typography>
  //     )
  //   }
  // },
  // {
  //   flex: 0.2,
  //   minWidth: 250,
  //   field: 'OrgLevel1Id',
  //   headerName: 'OrgLevel1 Id',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Typography noWrap variant='body2'>
  //         {row.OrgLevel1Id}
  //       </Typography>
  //     )
  //   }
  // },
  // {
  //   flex: 0.2,
  //   minWidth: 250,
  //   field: ' OrgLevel2Id',
  //   headerName: ' OrgLevel2 Id',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Typography noWrap variant='body2'>
  //         {row.OrgLevel2Id}
  //       </Typography>
  //     )
  //   }
  // },
  // {
  //   flex: 0.2,
  //   minWidth: 250,
  //   field: 'OrgLevel3Id',
  //   headerName: 'OrgLevel3 Id',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Typography noWrap variant='body2'>
  //         {row.OrgLevel3Id}
  //       </Typography>
  //     )
  //   }
  // },
  // {
  //   flex: 0.2,
  //   minWidth: 250,
  //   field: 'Org4Id',
  //   headerName: 'Org4 Id',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Typography noWrap variant='body2'>
  //         {row.Org4Id}
  //       </Typography>
  //     )
  //   }
  // },
  // {
  //   flex: 0.2,
  //   minWidth: 250,
  //   field: 'BrandId',
  //   headerName: 'Brand Id',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Typography noWrap variant='body2'>
  //         {row.BrandId}
  //       </Typography>
  //     )
  //   }
  // },
  // {
  //   flex: 0.2,
  //   minWidth: 250,
  //   field: 'MarketId',
  //   headerName: 'Market Id',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Typography noWrap variant='body2'>
  //         {row.MarketId}
  //       </Typography>
  //     )
  //   }
  // },
  // {
  //   flex: 0.2,
  //   minWidth: 250,
  //   field: 'ProvinceId',
  //   headerName: 'Province Id',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Typography noWrap variant='body2'>
  //         {row.ProvinceId}
  //       </Typography>
  //     )
  //   }
  // },

  // {
  //   flex: 0.2,
  //   minWidth: 250,
  //   field: 'CareerLevelId',
  //   headerName: 'Career Level Id',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Typography noWrap variant='body2'>
  //         {row.CareerLevelId}
  //       </Typography>
  //     )
  //   }
  // },
  // {
  //   flex: 0.2,
  //   minWidth: 250,
  //   field: 'ContractType',
  //   headerName: 'Contract Type',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Typography noWrap variant='body2'>
  //         {row.ContractType}
  //       </Typography>
  //     )
  //   }
  // },
  // {
  //   flex: 0.2,
  //   minWidth: 250,
  //   field: 'ContractRenewable',
  //   headerName: 'Contract Renewable',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Typography noWrap variant='body2'>
  //         {row.ContractRenewable}
  //       </Typography>
  //     )
  //   }
  // },
  // {
  //   flex: 0.2,
  //   minWidth: 250,
  //   field: 'NtsNoteId',
  //   headerName: 'Nts Note Id',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Typography noWrap variant='body2'>
  //         {row.NtsNoteId}
  //       </Typography>
  //     )
  //   }
  // },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'Mobile',
    headerName: 'Mobile',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.Mobile}
        </Typography>
      )
    }
  },
  // {
  //   flex: 0.2,
  //   minWidth: 250,
  //   field: 'ManagerPhoto',
  //   headerName: 'Manager Photo',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Typography noWrap variant='body2'>
  //         {row.ManagerPhoto}
  //       </Typography>
  //     )
  //   }
  // },
  // {
  //   flex: 0.2,
  //   minWidth: 250,
  //   field: 'LineManager',
  //   headerName: ' Line Manager',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Typography noWrap variant='body2'>
  //         {row.LineManager}
  //       </Typography>
  //     )
  //   }
  // },
  // {
  //   flex: 0.2,
  //   minWidth: 250,
  //   field: 'FinalScoreImage',
  //   headerName: 'Final Score Image',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Typography noWrap variant='body2'>
  //         {row.FinalScoreImage}
  //       </Typography>
  //     )
  //   }
  // },
  // {
  //   flex: 0.2,
  //   minWidth: 250,
  //   field: 'Id',
  //   headerName: 'Id',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Typography noWrap variant='body2'>
  //         {row.Id}
  //       </Typography>
  //     )
  //   }
  // },
  // {
  //   flex: 0.2,
  //   minWidth: 250,
  //   field: 'CreatedDate',
  //   headerName: 'Created Date',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Typography noWrap variant='body2'>
  //         {row.CreatedDate}
  //       </Typography>
  //     )
  //   }
  // },

  // {
  //   flex: 0.2,
  //   minWidth: 250,
  //   field: 'CreatedBy',
  //   headerName: 'Created By',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Typography noWrap variant='body2'>
  //         {row.CreatedBy}
  //       </Typography>
  //     )
  //   }
  // },
  // {
  //   flex: 0.2,
  //   minWidth: 250,
  //   field: 'LastUpdatedDate',
  //   headerName: 'Last Updated Date',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Typography noWrap variant='body2'>
  //         {row.LastUpdatedDate}
  //       </Typography>
  //     )
  //   }
  // },
  // {
  //   flex: 0.2,
  //   minWidth: 250,
  //   field: 'LastUpdatedBy',
  //   headerName: 'Last Updated By',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Typography noWrap variant='body2'>
  //         {row.LastUpdatedBy}
  //       </Typography>
  //     )
  //   }
  // },
  // {
  //   flex: 0.2,
  //   minWidth: 250,
  //   field: 'IsDeleted',
  //   headerName: 'Is Deleted',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Typography noWrap variant='body2'>
  //         {row.IsDeleted}
  //       </Typography>
  //     )
  //   }
  // },
  // {
  //   flex: 0.2,
  //   minWidth: 250,
  //   field: 'SequenceOrder',
  //   headerName: 'Sequence Order',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Typography noWrap variant='body2'>
  //         {row.SequenceOrder}
  //       </Typography>
  //     )
  //   }
  // },
  // {
  //   flex: 0.2,
  //   minWidth: 250,
  //   field: 'CompanyId',
  //   headerName: 'Company Id',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Typography noWrap variant='body2'>
  //         {row.CompanyId}
  //       </Typography>
  //     )
  //   }
  // },
  // {
  //   flex: 0.2,
  //   minWidth: 250,
  //   field: 'Status',
  //   headerName: 'Status',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Typography noWrap variant='body2'>
  //         {row.Status}
  //       </Typography>
  //     )
  //   }
  // },
  // {
  //   flex: 0.2,
  //   minWidth: 250,
  //   field: 'VersionNo',
  //   headerName: 'Version No',
  //   renderCell: ({ row }) => {
  //     return (
  //       <Typography noWrap variant='body2'>
  //         {row.VersionNo}
  //       </Typography>
  //     )
  //   }
  // },
  
  {
    flex: 0.1,
    minWidth: 90,
    sortable: false,
    field: 'actions',
    headerName: 'Actions',
    renderCell: ({ row }) => <RowOptions id={row.id} />
  }
]

const EmployeeDirectoryDashboard = () => {
   // ** State
   const [plan, setPlan] = useState('')
   const [value, setValue] = useState('')
   const [pageSize, setPageSize] = useState(10)
 
   // Api Intregration by using Get method
    const [getdata, setGetdata] = useState([]) 
   const viewData = async () => {
     let response = await axios.get(`https://webapidev.aitalkx.com/chr/hrdirect/GetEmployeeDirectory`)
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
    <Grid container sx={{ p: 5, pb: 3, flexWrap: 'flex', alignItems: 'center', justifyContent: 'space-between'}}>
     
      <Grid item xs={12}>
        <Card >
          <CardHeader title='Employee Directory' />
          <CardContent>
            <Grid spacing={6}>
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
          <Divider/>
          {/* <TableHeader value={value} handleFilter={handleFilter} /> */}
          
          <div style={{width: "1700px"}}>
          <DataGrid 
            autoHeight
            rows={getdata}
            columns={columns}
            checkboxSelection
            pageSize={pageSize}
            disableSelectionOnClick
            rowsPerPageOptions={[10, 25, 50]}
            onPageSizeChange={newPageSize => setPageSize(newPageSize)}
            getRowId={(row) => row.DataAction}
          />
          </div>
        </Card>
      </Grid>

      <AddUserDrawer />

    </Grid>
  )
}



export default EmployeeDirectoryDashboard 
