import { useEffect, useCallback, useState } from 'react'

// ** Next Import
import Link from 'next/link'

// ** MUI Imports
import Box from '@mui/material/Box'
import Card from '@mui/material/Card'
import Grid from '@mui/material/Grid'
import { DataGrid } from '@mui/x-data-grid'
import IconButton from '@mui/material/IconButton'
import Typography from '@mui/material/Typography'

// ** Icon Imports
import Icon from 'src/@core/components/icon'

// ** Store Imports
import { useDispatch, useSelector } from 'react-redux'

// ** Custom Components Imports
import CustomChip from 'src/@core/components/mui/chip'
import CustomAvatar from 'src/@core/components/mui/avatar'

// ** Utils Import
import { getInitials } from 'src/@core/utils/get-initials'

// ** Actions Imports
import { fetchData } from 'src/store/apps/user'

// ** Custom Components Imports
import TableHeader from './TableHeader'

//axios
import axios from 'axios'

function createData(Name, NoteNo, CreatedBy, CreatedDate, WorkflowStatus ) {
    return {Name, NoteNo, CreatedBy, CreatedDate, WorkflowStatus};
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

const columns = [

  {
    flex: 0.2,
    minWidth: 230,
    field: 'Name',
    headerName: 'Name',
    renderCell: ({ row }) => {
      const { fullName, username } = row

      return (
        <Box sx={{ display: 'flex', alignItems: 'center' }}>
          {/* {renderClient(row)} */}
          <Box sx={{ display: 'flex', alignItems: 'flex-start', flexDirection: 'column' }}>
            <Typography
              noWrap
              variant='body2'
              component={Link}
              href='/apps/user/view/overview/'
              sx={{
                fontWeight: 600,
                color: 'text.primary',
                textDecoration: 'none',
                '&:hover': { color: theme => theme.palette.primary.main }
              }}
            >

            </Typography>
            <Typography noWrap variant='caption'>
              {/* {row.Name} */}
              <p>No data found</p>
            </Typography>
          </Box>
        </Box>
      )
    }
  },

  {
    flex: 0.15,
    field: 'NoteNo',
    minWidth: 150,
    headerName: 'NoteNo',
    renderCell: ({ row }) => {
      return (
        <Box style={{ display: 'flex', alignItems: 'center' }}>
          <Icon fontSize={20} />
          <Typography noWrap sx={{ color: 'text.secondary', textTransform: 'capitalize' }}>
            {row.NoteNo}
          </Typography>
        </Box>
      )
    }
  },

  {
    flex: 0.15,
    minWidth: 120,
    headerName: 'Createdby',
    field: 'Createdby',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap sx={{ textTransform: 'capitalize' }}>
          {row.CreatedBy}
        </Typography>
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
          color={userStatusObj[row.EndDate]}
          sx={{ textTransform: 'capitalize' }}
        />
      )
    }
  },

  {
    flex: 0.15,
    field: 'WorkflowStatus',
    minWidth: 150,
    headerName: 'WorkflowStatus',
    renderCell: ({ row }) => {
      return (
        <Box style={{ display: 'flex', alignItems: 'center' }}>
          <Icon fontSize={20} />
          <Typography noWrap sx={{ color: 'text.secondary', textTransform: 'capitalize' }}>
            {/* {row.WorkflowStatus} */}
            <p>No data found</p>
          </Typography>
        </Box>
      )
    }
  },

]

const UserList = () => {
  // ** State
  const [plan, setPlan] = useState('')
  const [value, setValue] = useState('')
  const [pageSize, setPageSize] = useState(10)

  // Api Intregration by using Get method
  const [getdata, setGetdata] = useState([])
  const viewData = async () => {
    let response = await axios.get(`https://webapidev.aitalkx.com/dms/query/GetSourceFolders?key=0cdb29fa-de8a-403a-83a4-c30b702898b6&userId=45bba746-3309-49b7-9c03-b5793369d73c&portalName=HR`)
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
    <Grid container spacing={6} >
      <Grid item xs={12}>
        <Card>
          <TableHeader plan={plan} value={value} handleFilter={handleFilter} handlePlanChange={handlePlanChange} />
          <DataGrid
            autoHeight
            rows={getdata}
            columns={columns}
            checkboxSelection
            pageSize={pageSize}
            disableSelectionOnClick
            rowsPerPageOptions={[10, 25, 50]}
            onPageSizeChange={newPageSize => setPageSize(newPageSize)}
            getRowId={(row) => row.key}
      
          />    
        </Card>
      </Grid>
    </Grid>

  )
}

export default UserList
// ********************************************************************************

// // ** React Imports
// import { useState } from 'react'

// // ** MUI Imports
// import Box from '@mui/material/Box'
// import Card from '@mui/material/Card'
// import Typography from '@mui/material/Typography'
// import CardHeader from '@mui/material/CardHeader'
// import { DataGrid } from '@mui/x-data-grid'

// // ** Custom Components
// import CustomChip from 'src/@core/components/mui/chip'
// import CustomAvatar from 'src/@core/components/mui/avatar'
// import QuickSearchToolbar from 'src/views/table/data-grid/QuickSearchToolbar'

// // ** Utils Import
// import { getInitials } from 'src/@core/utils/get-initials'

// // ** Data Import
// import { rows } from 'src/@fake-db/table/static-data'

// // **useEffect
// import {useEffect} from 'react';

// // **axios import
// import axios from 'axios';

// function createData(Name, NoteNo, Createdby, createData, WorkflowStatus ) {
//   return {Name, NoteNo, Createdby, createData, WorkflowStatus};
// }

// // ** renders client column
// const renderClient = params => {
//   const { row } = params
//   const stateNum = Math.floor(Math.random() * 6)
//   const states = ['success', 'error', 'warning', 'info', 'primary', 'secondary']
//   const color = states[stateNum]
//   if (row.avatar.length) {
//     // row.avatar
//     return <CustomAvatar src={`/images/avatars/${row}`} sx={{ mr: 3, width: '1.875rem', height: '1.875rem' }} />
//   } else {
//     return (
//       <CustomAvatar skin='light' color={color} sx={{ mr: 3, fontSize: '.8rem', width: '1.875rem', height: '1.875rem' }}>
//         {getInitials(row.full_name ? row.full_name : 'John Doe')}
//       </CustomAvatar>
//     )
//   }
// }

// const statusObj = {
//   1: { title: 'current', color: 'primary' },
//   2: { title: 'professional', color: 'success' },
//   3: { title: 'rejected', color: 'error' },
//   4: { title: 'resigned', color: 'warning' },
//   5: { title: 'applied', color: 'info' }
// }

// const escapeRegExp = value => {
//   return value.replace(/[-[\]{}()*+?.,\\^$|#\s]/g, '\\$&')
// }

// const columns = [
//   {
//     flex: 0.275,
//     minWidth: 290,
//     field: 'Name',
//     headerName: 'Name',
//     renderCell: params => {
//       const { row } = params

//       return (
//         <Box sx={{ display: 'flex', alignItems: 'center' }}>
//           {renderClient(params)}
//           <Box sx={{ display: 'flex', flexDirection: 'column' }}>
//             <Typography noWrap variant='body2' sx={{ color: 'text.primary', fontWeight: 600 }}>
//               {row.Name}
//             </Typography>
//             <Typography noWrap variant='caption'>
//               {/* {row.email}  */}
//             </Typography>
//           </Box>
//         </Box>
//       )
//     }
//   },
//   {
//     flex: 0.2,
//     minWidth: 1,
//     headerName: 'Note no.',
//     field: 'NoteNo',
//     renderCell: params => (
//       <Typography variant='body2' sx={{ color: 'text.primary' }}>
//         {params.row.NoteNo}
//       </Typography>
//     )
//   },
//   {
//     flex: 0.2,
//     minWidth: 110,
//     field: 'Createdby',
//     headerName: 'Created By',
//     renderCell: params => (
//       <Typography variant='body2' sx={{ color: 'text.primary' }}>
//         {params.row.Createdby}
//       </Typography>
//     )
//   },
//   {
//     flex: 0.125,
//     field: 'createData',
//     minWidth: 80,
//     headerName: 'Created Date',
//     renderCell: params => (
//       <Typography variant='body2' sx={{ color: 'text.primary' }}>
//         {params.row.createData}
//       </Typography>
//     )
//   },
//   {
//     flex: 0.2,
//     minWidth: 140,
//     field: 'WorkflowStatus',
//     headerName: 'Workflow Status',
//     renderCell: params => {
//       // const status = statusObj[params.row.Workflowtatus]

//       return (
//         <CustomChip
//           size='small'
//           skin='light'
//           // color={status.color}
//           // label={status.title}
//           sx={{ '& .MuiChip-label': { textTransform: 'capitalize' } }}
//         />
//       )
//     }
//   }
// ]

// const Tble = () => {
//   // ** States
//   const [data] = useState(rows)
//   const [pageSize, setPageSize] = useState(7)
//   const [searchText, setSearchText] = useState('')
//   const [filteredData, setFilteredData] = useState([])

//   const handleSearch = searchValue => {
//     setSearchText(searchValue)
//     const searchRegex = new RegExp(escapeRegExp(searchValue), 'i')

//     const filteredRows = data.filter(row => {
//       return Object.keys(row).some(field => {
//         // @ts-ignore
//         return searchRegex.test(row[field].toString())
//       })
//     })
//     if (searchValue.length) {
//       setFilteredData(filteredRows)
//     } else {
//       setFilteredData([])
//     }
//   }

//  // Api Intregration by using Get method
//  const [getdata, setGetdata] = useState([])
//  const viewData = async () => {
  
//   //  let response = await axios.get(`https://webapidev.aitalkx.com/dms/query/GetDMSTree?UserID=45bba746-3309-49b7-9c03-b5793369d73c`)  
//   let response = await axios.get(`https://webapidev.aitalkx.com/dms/query/GetSourceFolders?key=0cdb29fa-de8a-403a-83a4-c30b702898b6&userId=45bba746-3309-49b7-9c03-b5793369d73c&portalName=HR`)  
  
  
//   setGetdata(response.data)
//   //  console.log(response.data, "response data")
//  }
//  console.log(getdata, 'response')

//  useEffect(() => {
//    viewData()
//  }, [])


//   return (
//     <Card sx={{width:"100%"}}>
//       <CardHeader title='Quick Filter' />
//       <DataGrid
//         autoHeight
//         columns={columns}
//         pageSize={pageSize}
//         rowsPerPageOptions={[7, 10, 25, 50]}
//         components={{ Toolbar: QuickSearchToolbar }}
//         rows={filteredData.length ? filteredData : data}
//         onPageSizeChange={newPageSize => setPageSize(newPageSize)}
//         componentsProps={{
//           baseButton: {
//             variant: 'outlined'
//           },
//           toolbar: {
//             value: searchText,
//             clearSearch: () => handleSearch(''),
//             onChange: event => handleSearch(event.target.value)
//           }
//         }}
//       />
//     </Card>
//   )
// }

// export default Tble