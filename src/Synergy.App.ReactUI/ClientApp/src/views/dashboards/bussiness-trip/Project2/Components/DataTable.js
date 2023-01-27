

// ** React Imports
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
import BuisnesHeader from './BuisnesHeader'


//axios
import axios from 'axios'

function createData(ServiceNo,Id ,StartDate,Status,ClaimServiceNo) {
  return { ServiceNo,Id ,StartDate,Status,ClaimServiceNo};
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

const renderClient = row => {

}





const columns = [
  
  {
    flex: 0.2,
    minWidth: 230,
    field: 'ServiceNo',
    headerName: 'ServiceNo',
    renderCell: ({ row }) => {
      const { fullName, username } = row

      return (
       
        <Box sx={{ display: 'flex', alignItems: 'center' }}>
         
          {renderClient(row)}
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
            {row.ServiceNo}
            </Typography>
          </Box>
        </Box>
      )
    }
  },
 
  {
    flex: 0.15,
    field: 'Id',
    minWidth: 150,
    headerName: 'Id',
    renderCell: ({ row }) => {
      return (
        <Box style={{ display: 'flex', alignItems: 'center' }}>
          <Icon fontSize={20} />
          <Typography noWrap sx={{ color: 'text.secondary', textTransform: 'capitalize' }}>
            {row.Id}
          </Typography>
        </Box>
      )
    }
  },
  
  {
    flex: 0.15,
    minWidth: 120,
    headerName: 'StartDate',
    field: 'StartDate',
    renderCell: ({ row }) => {
      return (
        <Typography noWrap sx={{ textTransform: 'capitalize' }}>
          {row.StartDate}
        </Typography>
      )
    }
  },
  
  {
    flex: 0.1,
    minWidth: 110,
    field: 'Status',
    headerName: 'Status',
    renderCell: ({ row }) => {
      return (
        <CustomChip
          skin='light'
          size='small'
          label={row. Purpose}
          color={userStatusObj[row.Status]}
          sx={{ textTransform: 'capitalize' }}
        />
      )
    }
  },
  
  {
    flex: 0.15,
    field: 'ClaimServiceNo',
    minWidth: 150,
    headerName: 'ClaimServiceNo',
    renderCell: ({ row }) => {
      return (
        <Box style={{ display: 'flex', alignItems: 'center' }}>
          <Icon fontSize={20} />
          <Typography noWrap sx={{ color: 'text.secondary', textTransform: 'capitalize' }}>
            {row.ClaimServiceNo}
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
    let response = await axios.get(`https://webapidev.aitalkx.com/chr/leave/GetBusinessTripData?portalName=HR&userId=45bba746-3309-49b7-9c03-b5793369d73c`)
    setGetdata(response.data)
    //console.log(response.data, "response data")
  }
  
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
  console.log("getdata", getdata)



  return (
    <Grid container spacing={6}>
      <Grid item xs={12}>
        <Card>
          <BuisnesHeader plan={plan} value={value} handleFilter={handleFilter} handlePlanChange={handlePlanChange} />
                <DataGrid
                  autoHeight
                  rows={getdata}
                  columns={columns}
                  checkboxSelection
                  pageSize={pageSize}
                  disableSelectionOnClick
                  rowsPerPageOptions={[10, 25, 50]}
                  onPageSizeChange={newPageSize => setPageSize(newPageSize)}
                  getRowId={(row) =>  row.Id}

                  
                  
                />
        </Card>
       
      </Grid>
    </Grid>
  )
}

export default UserList
