// ** React Imports
import { useState, useEffect, useCallback } from 'react'

// ** Next Imports
import Link from 'next/link'

// ** MUI Imports
// import Box from '@mui/material/Box'
import Typography from '@mui/material/Typography'
import Card from '@mui/material/Card'
import Grid from '@mui/material/Grid'
// import Divider from '@mui/material/Divider'
import { DataGrid } from '@mui/x-data-grid'
import { styled } from '@mui/material/styles'

// ** Third Party Components
import axios from 'axios'

function createData(PersonFullName, SponsorshipNo, BiometricId, PunchingTime, DevicePunchingTypeText, DeviceName) {
  return { PersonFullName, SponsorshipNo, BiometricId, PunchingTime, DevicePunchingTypeText, DeviceName }
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
    field: "'PersonFullName'",
    headerName: "'Person Full Name'",
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
    field: 'SponsorshipNo',
    headerName: 'Sponsorship No'
  },
  {
    flex: 0.2,
    field: 'BiometricId',
    headerName: 'Biometric Id',
    minWidth: 200
  },

  {
    flex: 0.2,
    field: 'PunchingTime',
    headerName: 'Punching Time',
    minWidth: 200
  },

  {
    flex: 0.2,
    field: 'DevicePunchingTypeText',
    headerName: 'Device Punching TypeText',
    minWidth: 200
  },
  {
    flex: 0.2,
    field: 'DeviceName',
    headerName: 'Device Name',
    minWidth: 200
  }
]

const OtherReimbTable = ({ apiData }) => {
  // ** State
  const [role, setRole] = useState('')
  const [plan, setPlan] = useState('')

  const [pageSize, setPageSize] = useState(10)
  const [addUserOpen, setAddUserOpen] = useState(false)

  // Api intergration by using get method
  const [getdata, setGetdata] = useState([])
  const viewData = async () => {
    let response = await axios.get(`https://webapidev.aitalkx.com/taa/query/AccessLogList`)
    setGetdata(response.data)
  }

  console.log(getdata, 'response')
  useEffect(() => {
    viewData()
  }, [])

  return (
    <div>
      <Grid item xs={12}>
        <Card>
          <DataGrid
            autoHeight
            rows={getdata}
            columns={columns}
            pageSize={pageSize}
            disableSelectionOnClick
            rowsPerPageOptions={[10, 15, 50]}
            onPageSizeChange={newPageSize => setPageSize(newPageSize)}
            getRowId={row => row.Id}
          />
        </Card>
      </Grid>
    </div>
  )
}

export default OtherReimbTable
