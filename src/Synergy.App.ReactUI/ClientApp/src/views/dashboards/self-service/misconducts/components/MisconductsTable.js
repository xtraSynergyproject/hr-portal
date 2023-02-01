// ** React Imports
import { useState, useEffect, useCallback } from 'react'

// ** Next Imports
import Link from 'next/link'

// ** MUI Imports
// import Box from '@mui/material/Box'
import Card from '@mui/material/Card'
import Grid from '@mui/material/Grid'
// import Divider from '@mui/material/Divider'
import { DataGrid } from '@mui/x-data-grid'
import { styled } from '@mui/material/styles'

import Icon from 'src/@core/components/icon'

// ** Store Imports
import { useDispatch, useSelector } from 'react-redux'

// ** Custom Components Imports
import CardStatisticsHorizontal from 'src/@core/components/card-statistics/card-stats-horizontal'

// ** Actions Imports
import { fetchData, deleteUser } from 'src/store/apps/user'

// ** Third Party Components
import axios from 'axios'

function createData(WorkspaceName, ParentName, LegalEntityName, CreatedbyName) {
  return { WorkspaceName, ParentName, LegalEntityName, CreatedbyName }
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
//change1
// const renderClient = row => {

// }

// const RowOptions = ({ id }) => {
//   // ** Hooks
//   // const dispatch = useDispatch()

//   // ** State
//   // const [anchorEl, setAnchorEl] = useState(null)
//   // const rowOptionsOpen = Boolean(anchorEl)

//   // const handleRowOptionsClick = event => {
//   //   setAnchorEl(event.currentTarget)
//   // }

//   // const handleRowOptionsClose = () => {
//   //   setAnchorEl(null)
//   // }

//   // const handleDelete = () => {
//   //   dispatch(deleteUser(id))
//   //   handleRowOptionsClose()
//   // }

//   return (
//     <>
// {/*
//       <IconButton size='small' onClick={handleRowOptionsClick}>
//         <Icon icon='mdi:dots-vertical' />
//       </IconButton>
//       <Menu
//         keepMounted
//         anchorEl={anchorEl}
//         open={rowOptionsOpen}
//         onClose={handleRowOptionsClose}
//         anchorOrigin={{
//           vertical: 'bottom',
//           horizontal: 'right'
//         }}
//         transformOrigin={{
//           vertical: 'top',
//           horizontal: 'right'
//         }}
//         PaperProps={{ style: { minWidth: '8rem' } }}
//       >
//         <MenuItem
//           component={Link}
//           sx={{ '& svg': { mr: 2 } }}
//           onClick={handleRowOptionsClose}
//           href='/apps/user/view/overview/'
//         >
//           <Icon icon='mdi:eye-outline' fontSize={20} />
//           View
//         </MenuItem>
//         <MenuItem onClick={handleRowOptionsClose} sx={{ '& svg': { mr: 2 } }}>
//           <Icon icon='mdi:pencil-outline' fontSize={20} />
//           Edit
//         </MenuItem>
//         <MenuItem onClick={handleDelete} sx={{ '& svg': { mr: 2 } }}>
//           <Icon icon='mdi:delete-outline' fontSize={20} />
//           Delete
//         </MenuItem>
//       </Menu> */}
//     </>
//   )
// }

const columns = [
  {
    flex: 0.2,
    minWidth: 200,
    field: 'Service No.',
    headerName: 'Service No.'
    // renderCell: ({ row }) => {
    //   // const { fullName, WorkspaceName } = row

    //   return (
    //     <Box sx={{ display: 'flex', alignItems: 'center' }}>
    //       {renderClient(row)}
    //       <Box sx={{ display: 'flex', alignItems: 'flex-start', flexDirection: 'column' }}>
    //         <StyledLink href='/apps/user/view/overview/'>{}</StyledLink>
    //         {row.WorkspaceName}
    //         <Typography noWrap variant='caption'>

    //         </Typography>
    //       </Box>
    //     </Box>

    //   )
    // }
  },
  {
    flex: 0.2,
    minWidth: 200,
    field: 'Misconduct Type Name',
    headerName: 'Misconduct Type Name'
    // renderCell: ({ row }) => {
    //   return (
    //     <Typography noWrap variant='body2'>
    //       {/* {row.} */}
    //     </Typography>
    //   )
    // }
  },
  {
    flex: 0.2,
    field: 'Disciplinary Action Taken Name',
    headerName: 'Disciplinary Action Taken Name',
    minWidth: 200
    // renderCell: ({ row }) => {
    //   return (
    //     <Box sx={{ display: 'flex', alignItems: 'center' }}>
    //       {/* <Icon icon={userRoleObj[row.role].icon} fontSize={20} /> */}
    //       <Typography noWrap sx={{ color: 'text.secondary', textTransform: 'capitalize' }}>
    //       {row.LegalEntityName}
    //       </Typography>
    //     </Box>
    //   )
    // }
  },

  {
    flex: 0.2,
    field: 'Misconduct Date',
    headerName: 'Misconduct Date',
    minWidth: 200
    // renderCell: ({ row }) => {
    //   return (
    //     <Box sx={{ display: 'flex', alignItems: 'center' }}>
    //       {/* <Icon icon={userRoleObj[row.role].icon} fontSize={20} /> */}
    //       <Typography noWrap sx={{ color: 'text.secondary', textTransform: 'capitalize' }}>
    //       {row.LegalEntityName}
    //       </Typography>
    //     </Box>
    //   )
    // }
  },
  {
    flex: 0.2,
    field: 'Status',
    headerName: 'Status',
    minWidth: 200
    // renderCell: ({ row }) => {
    //   return (
    //     <Box sx={{ display: 'flex', alignItems: 'center' }}>
    //       {/* <Icon icon={userRoleObj[row.role].icon} fontSize={20} /> */}
    //       <Typography noWrap sx={{ color: 'text.secondary', textTransform: 'capitalize' }}>
    //       {row.LegalEntityName}
    //       </Typography>
    //     </Box>
    //   )
    // }
  }
  
  


]

const MisconductsTable = ({ apiData }) => {
  // ** State
  const [role, setRole] = useState('')
  const [plan, setPlan] = useState('')

  const [pageSize, setPageSize] = useState(10)
  const [addUserOpen, setAddUserOpen] = useState(false)

  //change2
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
  // }, [dispatch, plan, value])

  // const handleFilter = useCallback(val => {
  //   setValue(val)
  // }, [])

  // const handleRoleChange = useCallback(e => {
  //   setRole(e.target.value)
  // }, [])

  // const handlePlanChange = useCallback(e => {
  //   setPlan(e.target.value)
  // }, [])

  // const handleStatusChange = useCallback(e => {
  //   setStatus(e.target.value)
  // }, [])
  // const toggleAddUserDrawer = () => setAddUserOpen(!addUserOpen)

  // Api Intregration by using Get method
  const [getdata, setGetdata] = useState([])
  const viewData = async () => {
    // let response = await axios.get(`https://webapidev.aitalkx.com/api/Command/CreateWorkspace`)
    let response = await axios.get(
      `https://webapidev.aitalkx.com/dms/workspace/ReadDataGrid?userId=45bba746-3309-49b7-9c03-b5793369d73c&portalName=HR`
    )

    setGetdata(response.data)
    //  console.log(response.data, "response data")
  }
  console.log(getdata, 'response')

  useEffect(() => {
    viewData()
  }, [])

  return (
    <div>
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
            {/* <CardHeader title='Workspace' /> */}

            {/* <Divider /> */}
            {/* <TableHeader  handleFilter={handleFilter} toggle={toggleAddUserDrawer} /> */}

            <DataGrid
              autoHeight
              rows={getdata}
              columns={columns}
              checkboxSelection
              pageSize={pageSize}
              disableSelectionOnClick
              rowsPerPageOptions={[10, 25, 50]}
              onPageSizeChange={newPageSize => setPageSize(newPageSize)}
              getRowId={row => row.Id}
            />
          </Card>
        </Grid>
      </Grid>
    </div>
  )
}

export default MisconductsTable
