import { useEffect, useCallback, useState } from 'react'

// ** Next Import
import Link from 'next/link'

// ** MUI Imports
import {Box,Button} from '@mui/material'
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

// ** Utils Import
import { getInitials } from 'src/@core/utils/get-initials'

// ** Actions Imports
import { fetchData } from 'src/store/apps/user'

// ** Custom Components Imports
import TableHeader from 'src/views/apps/roles/TableHeader'

//axios
import axios from 'axios'

function createData(ServiceNo,ServiceName, ServiceStatusName,TaskNo,TaskSubject,TaskStatusName,RequestedBy,AssignedTo,RequestedDate) {
  return {ServiceNo,ServiceName,ServiceStatusName,TaskNo,TaskSubject,TaskStatusName,RequestedBy,AssignedTo,RequestedDate}
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
    field: 'ServiceNo',
    headerName: 'Service No',
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
            ></Typography>
            <Typography noWrap variant='caption'>
              {row.ServiceNo}
            </Typography>
          </Box>
        </Box>
      )
    }
  },

  {
    flex: 0.15,
    field: 'ServiceName',
    minWidth: 150,
    headerName: 'Service ',
    renderCell: ({ row }) => {
      return (
        <Box style={{ display: 'flex', alignItems: 'center' }}>
          <Icon fontSize={20} />
          <Typography noWrap sx={{ color: 'text.secondary', textTransform: 'capitalize' }}>
            {row.ServiceName}
          </Typography>
        </Box>
      )
    }
  },

  {
    flex: 0.15,
    minWidth: 120,
    headerName: 'Service Status Name',
    field: 'ServiceStatusName',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap sx={{ textTransform: 'capitalize' }}>
          {row.ServiceStatusName}
        </Typography>
      )
    }
  },

  {
    flex: 0.1,
    minWidth: 110,
    field: 'TaskNo',
    headerName: 'Task No',
    renderCell: ({ row }) => {
      return (
        <CustomChip
          skin='light'
          size='small'
          label={row.TaskNo}
          color={userStatusObj[row.TaskNo]}
          sx={{ textTransform: 'capitalize' }}
        />
      )
    }
  },
  {
    flex: 0.1,
    minWidth: 110,
    field: 'TaskSubject',
    headerName: 'Task Subject',
    renderCell: ({ row }) => {
      return (
        <CustomChip
          skin='light'
          size='small'
          label={row.TaskSubject}
          color={userStatusObj[row.TaskSubject]}
          sx={{ textTransform: 'capitalize' }}
        />
      )
    }
  },
  {
    flex: 0.1,
    minWidth: 110,
    field: 'TaskStatusName',
    headerName: 'Task Status',
    renderCell: ({ row }) => {
      return (
        <CustomChip
          skin='light'
          size='small'
          label={row.TaskStatusName}
          color={userStatusObj[row.TaskStatusName]}
          sx={{ textTransform: 'capitalize' }}
        />
      )
    }
  },
  {
    flex: 0.1,
    minWidth: 110,
    field: 'AssignedTo',
    headerName: 'Assigned To',
    renderCell: ({ row }) => {
      return (
        <CustomChip
          skin='light'
          size='small'
          label={row.AssignedTo}
          color={userStatusObj[row.AssignedTo]}
          sx={{ textTransform: 'capitalize' }}
        />
      )
    }
  },

  {
    flex: 0.1,
    minWidth: 110,
    field: 'RequestedDate',
    headerName: 'Requested Date',
    renderCell: ({ row }) => {
      return (
        <CustomChip
          skin='light'
          size='small'
          label={row.RequestedDate}
          color={userStatusObj[row.RequestedDate]}
          sx={{ textTransform: 'capitalize' }}
        />
      )
    }
  },




  
]

const UserList = () => {
  // ** State
  const [plan, setPlan] = useState('')
  const [value, setValue] = useState('')
  const [pageSize, setPageSize] = useState(10)
  const [getdata, setGetdata] = useState([])
  const viewData = async () => {
  

    try {
      let response = await axios.get(
         'https://webapidev.aitalkx.com/cms/task/ReadTaskData?userId=45bba746-3309-49b7-9c03-b5793369d73c&portalName=HR'
      )
      console.log(response);
      setGetdata(response.data)
    }catch(error) {
      console.log(error);
    }
    
  }
  console.log(getdata, 'response')

  useEffect(() => {
    viewData()
  }, [])


  
  
  const handleFilter = useCallback(val => {
    setValue(val)
  }, [])

  const handlePlanChange = useCallback(e => {
    setPlan(e.target.value)
  }, [])

  return (
    <>
      <Grid container spacing={6}>
        <Grid item xs={11}>
          <Box sx={{ width: 'auto', margin: 5 }}>
            <Box sx={{ display: 'flex', justifyContent: 'space-between', textAlign: 'left' }}>
              <Box sx={{ mt: 5, fontWeight: 5, minWidth: '30%' }}>
                <Button>Clear Filter</Button>
                {/* <Typography>
                  <h3>Leave Transaction Details</h3>
                </Typography> */}
              </Box>
              <Box sx={{ mt: 5, fontWeight: 5, minWidth: '5%' }}>
                
              </Box>
            </Box>
          </Box>
        </Grid>
        <Grid item xs={12}>
          <Card>
            {/* <TableHeader plan={plan} value={value} handleFilter={handleFilter} handlePlanChange={handlePlanChange} /> */}
            <DataGrid
              autoHeight
              rows={getdata}
              columns={columns}
              // checkboxSelection
              pageSize={pageSize}
              disableSelectionOnClick
              rowsPerPageOptions={[10, 25, 50]}
              onPageSizeChange={newPageSize => setPageSize(newPageSize)}
              getRowId={row => row.ServiceNo}
            />
          </Card>
        </Grid>
      </Grid>
    </>
  )
}

export default UserList