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
import Typography from '@mui/material/Typography'

import Icon from 'src/@core/components/icon'

// ** Custom Components Imports
import CardStatisticsHorizontal from 'src/@core/components/card-statistics/card-stats-horizontal'

// ** Actions Imports
import { fetchData, deleteUser } from 'src/store/apps/user'

// ** Third Party Components
import axios from 'axios'

function createData(ServiceNo, ServiceOwner,SignInTime, SignOutTime,Status) {
  return { ServiceNo, ServiceOwner, SignInTime, SignOutTime,Status }
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

const columns = [
  {
    flex: 0.2,
    minWidth: 200,
    field: 'ServiceNo',
    headerName: 'Service No',
    renderCell: ({ row }) => {
      const { fullName, username } = row
      return (
        <Typography noWrap variant='body2'>
          {row.PersonFullName}
        </Typography>
      )
    }
  },

  {
    flex: 0.2,
    minWidth: 200,
    field: 'ServiceOwner',
    headerName: 'Service Owner'
  },

  {
    flex: 0.2,
    field: 'SignInTime',
    headerName: 'SignIn Time',
    minWidth: 200
  },

  {
    flex: 0.2,
    field: 'SignOutTime',
    headerName: 'SignOut Time',
    minWidth: 200
  },

  {
    flex: 0.2,
    field: 'Status',
    headerName: 'Status',
    minWidth: 200
  }
]

const OtherReimbTable = ({ apiData }) => {
  // ** State
  const [role, setRole] = useState('')
  const [plan, setPlan] = useState('')

  const [pageSize, setPageSize] = useState(10)
  const [addUserOpen, setAddUserOpen] = useState(false)

  // Api Intregration by using Get method
  const [getdata, setGetdata] = useState([])
  const viewData = async () => {
    let response = await axios.get(
      `https://webapidev.aitalkx.com/chr/hrdirect/GetRemoteSignInSingOutGridData?Id=45bba746-3309-49b7-9c03-b5793369d73c&UserId=45bba746-3309-49b7-9c03-b5793369d73c`
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

export default OtherReimbTable
