// ** React Imports
import { useState, useEffect, useCallback } from 'react'
// import './Dropdown.js'

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
import Button from '@mui/material/Button'
import CardHeader from '@mui/material/CardHeader'
import InputLabel from '@mui/material/InputLabel'
import FormControl from '@mui/material/FormControl'
import CardContent from '@mui/material/CardContent'
import Select from '@mui/material/Select'
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
function createData(ParentDepartmentId,DepartmentName,DepartmentNameArabic,DepartmentCategoryId,DepartmentAdminId,ResponsibilityCenterId,IsPayrollDepartment,CreatedByUser_Name,CreatedDate,CostCenterId){
  return createData(ParentDepartmentId,DepartmentName,DepartmentNameArabic,DepartmentCategoryId,DepartmentAdminId,ResponsibilityCenterId,IsPayrollDepartmentCreatedByUser_Name,CreatedDate,CostCenterId)
}

// ** Custom Table Components Imports
import TableHeader from 'src/views/apps/user/list/TableHeader'
import AddUserDrawer from 'src/views/apps/user/list/AddUserDrawer'
// import Model from './Model'

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
  //       {getInitials(row.fullName ? row.fullName : 'John Doe')}
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
          horizontal: 'right'
        }}
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
    minWidth: 230,
    field: 'ParentDepartmentId',
    headerName: 'Parent Department Name',
    renderCell: ({ row }) => {
      const { fullName, username } = row

      return (
        <>
          {/* <Box sx={{ display: 'flex', alignItems: 'center' }}>
            {renderClient(row)}
            <Box sx={{ display: 'flex', alignItems: 'flex-start', flexDirection: 'column' }}>
              <StyledLink href='/apps/user/view/overview/'>{}</StyledLink>
              {LocationName}
              <Typography noWrap variant='caption'>
                {`@${username}`}
              </Typography>
            </Box>
          </Box> */}
          {/* <Model /> */}
          {row.ParentDepartmentId}
        </>
      )
    }
  },
  {
    flex: 0.2,
    minWidth: 250,
    field: 'DepartmentName',
    headerName: 'DepartmentName',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap variant='body2'>
          {row.DepartmentName}
        </Typography>
      )
    }
  },
  {
    flex: 0.15,
    field: 'DepartmentNameArabic',
    minWidth: 150,
    headerName: 'Department Name Arabic',
    renderCell: ({ row }) => {
      return (
        
         <Typography noWrap sx={{ textTransform: 'capitalize' }}>
          {row.DepartmentNameArabic}
        </Typography>
      )
    }
  },
  {
    flex: 0.15,
    minWidth: 120,
    headerName: 'Department Category',
    field: 'DepartmentCategoryId',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap sx={{ textTransform: 'capitalize' }}>
          {row.DepartmentCategoryId}
        </Typography>
      )
    }
  },


  {
    flex: 0.1,
    minWidth: 110,
    field: 'DepartmentAdminId',
    headerName: 'Department Admin Name',
    renderCell: ({ row }) => {
      return (
        <CustomChip
          skin='light'
          size='small'
          field={row.DepartmentAdminId}
         
          sx={{ textTransform: 'capitalize' }}
        />
      )
    }
  },
  {
    flex: 0.1,
    minWidth: 110,
    field: 'CostCenterId',
    headerName: 'cost center Name',
    renderCell: ({ row }) => {
      return (
        <CustomChip
          skin='light'
          size='small'
          label={row.CostCenterId}
         
          sx={{ textTransform: 'capitalize' }}
        />
      )
    }
  },

  {
    flex: 0.1,
    minWidth: 110,
    field: 'ResponsibilityCenterId',
    headerName: 'ResponsibilityCenterId',
    renderCell: ({ row }) => {
      return (
        <CustomChip
          skin='light'
          size='small'
          label={row.ResponsibilityCenterId}
         
          sx={{ textTransform: 'capitalize' }}
        />
      )
    }
  },
  {
    flex: 0.1,
    minWidth: 110,
    field: 'IsPayrollDepartment',
    headerName: 'IsPayrollDepartment',
    renderCell: ({ row }) => {
      return (
        <CustomChip
          skin='light'
          size='small'
          label={row.IsPayrollDepartment}
         
          sx={{ textTransform: 'capitalize' }}
        />
      )
    }
  },
  {
    flex: 0.1,
    minWidth: 110,
    field: 'CreatedByUser_Name',
    headerName: 'CreatedByUser_Name',
    renderCell: ({ row }) => {
      return (
        <CustomChip
          skin='light'
          size='small'
          label={row.CreatedByUser_Name}
         
          sx={{ textTransform: 'capitalize' }}
        />
      )
    }
  },
  {
    flex: 0.1,
    minWidth: 110,
    field: 'CreatedDate',
    headerName: 'CreatedDate',
    renderCell: ({ row }) => {
      return (
        <CustomChip
          skin='light'
          size='small'
          label={row.CreatedDate}
         
          sx={{ textTransform: 'capitalize' }}
        />
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

const UserList = ({ apiData }) => {
  // ** State
  const [role, setRole] = useState('')
  const [plan, setPlan] = useState('')
  // const [value, setValue] = useState('')
  // const [status, setStatus] = useState('')
   const [pageSize, setPageSize] = useState(10)
 const [addUserOpen, setAddUserOpen] = useState(false)
  // Api intergration by using get method
  const[getdata, setGetdata]=useState([])
  const viewData=async()=>{
    let response = await axios.get(`https://webapidev.aitalkx.com/CHR/query/LoadNoteIndexPageGrid?indexPageTemplateId=b12c4ce9-9cff-48fe-b39d-bd363698abf2&userId=45bba746-3309-49b7-9c03-b5793369d73c`)
    setGetdata(response.data)
  }

  console.log(getdata, "response");
  useEffect(()=>{
    viewData()
  },[])

  // // ** Hooks
  // const dispatch = useDispatch()
  // const store = useSelector(state => state.user)
  // useEffect(() => {
  //   dispatch(
  //     fetchData({
  //       role,
  //       status,
  //       q: value,
  //       currentPlan: plan
  //     })
  //   )
  // }, [dispatch, plan, role, status, value])

  const handleFilter = useCallback(val => {
    setValue(val)
  }, [])

  const handleRoleChange = useCallback(e => {
    setRole(e.target.value)
  }, [])

  const handlePlanChange = useCallback(e => {
    setPlan(e.target.value)
  }, [])

  const handleStatusChange = useCallback(e => {
    setStatus(e.target.value)
  }, [])
  const toggleAddUserDrawer = () => setAddUserOpen(!addUserOpen)

  return (
    <Grid container spacing={6}>
      <Grid item xs={12}>
        {apiData && (
          <Grid container spacing={6}>
            {apiData.statsHorizontal.map((item, index) => {
              return (
                <Grid item xs={12} md={3} sm={6} key={index}>
                  <CardStatisticsHorizontal {...item} icon={<Icon icon={item.icon} />} />
                </Grid>
              )
            })}
          </Grid>
        )}
      </Grid>
      <Grid item xs={12}>
        <Card>
          <CardHeader title='Department' />
          <CardContent>
            <Grid container spacing={6}>
              {/* <Grid item sm={4} xs={12}> */}
                {/* <FormControl fullWidth>
                  <InputLabel id='role-select'>Import/Export</InputLabel>
                  <Select
                    fullWidth
                    value={role}
                    id='select-role'
                    label='Select Role'
                    labelId='role-select'
                    onChange={handleRoleChange}
                    inputProps={{ placeholder: 'Select Role' }}
                    startIcon={<Icon icon='mdi:export-variant' fontSize={20} />}
                  >
                    <MenuItem value=''>Select </MenuItem>
                    <MenuItem value='admin'>Download Template</MenuItem>
                    <MenuItem value='author'>Download Data</MenuItem>
                    <MenuItem value='editor'>Upload Template</MenuItem>
                    <MenuItem value='maintainer'>Maintainer</MenuItem>
                    <MenuItem value='subscriber'>Subscriber</MenuItem>
                  </Select>
                </FormControl> */}
              </Grid>
              {/* <Grid item sm={4} xs={12}>
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
              </Grid> */}
              {/* <Grid item sm={4} xs={12}>
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
            {/* </Grid> */}
          </CardContent>
          <Divider />
          <TableHeader  handleFilter={handleFilter} toggle={toggleAddUserDrawer} />
          <DataGrid
            autoHeight
            rows={getdata}
            columns={columns}
            checkboxSelection
            pageSize={pageSize}
            disableSelectionOnClick
            rowsPerPageOptions={[10, 25, 50]}
            onPageSizeChange={newPageSize => setPageSize(newPageSize)}
            getRowId={(row)=> row.Id}
          />
        </Card>
      </Grid>

      <AddUserDrawer open={addUserOpen} toggle={toggleAddUserDrawer} />
    </Grid>
  )
}

export const getStaticProps = async () => {
  const res = await axios.get('/cards/statistics')
  const apiData = res.data

  return {
    props: {
      apiData
    }
  }
}

export default UserList








