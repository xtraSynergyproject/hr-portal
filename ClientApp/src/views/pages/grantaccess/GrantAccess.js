import * as React from 'react'

// ** MUI Imports
import Card from '@mui/material/Card'
import Menu from '@mui/material/Menu'
import Grid from '@mui/material/Grid'
import { DataGrid } from '@mui/x-data-grid'
import { styled } from '@mui/material/styles'
import MenuItem from '@mui/material/MenuItem'
import IconButton from '@mui/material/IconButton'
import Typography from '@mui/material/Typography'
import CardHeader from '@mui/material/CardHeader'
import Box from '@mui/material/Box'
import DialogContent from '@mui/material/DialogContent'
import Modal from '@mui/material/Modal'
import DialogTitle from '@mui/material/DialogTitle'
import { Divider } from '@mui/material'

import Icon from 'src/@core/components/icon'

// ** React Imports
import { useState, useEffect, useCallback } from 'react'

// ** Next Imports
import Link from 'next/link'

// ** Store Imports
import { useDispatch } from 'react-redux'

// ** Custom Components Imports
import CardStatisticsHorizontal from 'src/@core/components/card-statistics/card-stats-horizontal'

// ** Actions Imports
import { deleteUser } from 'src/store/apps/user'

// ** Custom Table Components Imports
import AddUserDrawer from './components/imports/AddUserDrawer'
import Adduser from './components/imports/AddUser'

// ** Third Party Components
import axios from 'axios'

function createData(UserName, StartDate, EndDate, GrantStatus) {
  return { UserName, StartDate, EndDate, GrantStatus }
}

const modalWrapper = {
  position: 'fixed',
  overflow: 'scroll',
  maxHeight: '90vh',
  display: 'flex',
  top: '5%'
}

const modalBlock = {
  position: 'relative',
  zIndex: 0,
  display: 'flex',
  alignItems: 'center',
  justifyContent: 'center',
  margin: 'auto'
}

const modalContentStyle = {
  position: 'relative',
  background: '#fff',
  width: '80rem',
  boxShadow: 2,
  borderRadius: '10px'
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
const renderClient = row => {}

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
    flex: 0.15,
    minWidth: 200,
    field: 'UserNamee',
    headerName: 'UserName',
    renderCell: ({ row }) => {
      // const { fullName, WorkspaceName } = row

      return (
        <Box sx={{ display: 'flex', alignItems: 'center' }}>
          <Typography noWrap sx={{ color: 'text.secondary', textTransform: 'capitalize' }}>
            {'No data found'}
          </Typography>
        </Box>
      )
    }
  },
  {
    flex: 0.15,
    minWidth: 200,
    field: 'StartDate',
    headerName: 'StartDate',
    renderCell: ({ row }) => {
      return (
        <Box sx={{ display: 'flex', alignItems: 'center' }}>
          <Typography noWrap sx={{color: 'text.secondary', textTransform: 'capitalize' }}>
            {'No data found'}
          </Typography>
        </Box>
      )
    }
  },
  {
    flex: 0.15,
    field: 'EndDate',
    minWidth: 200,
    headerName: 'EndDate',
    renderCell: ({ row }) => {
      return (
        <Box sx={{ display: 'flex', alignItems: 'center' }}>
          {/* <Icon icon={userRoleObj[row.role].icon} fontSize={20} /> */}
          <Typography noWrap sx={{ color: 'text.secondary', textTransform: 'capitalize' }}>
            {'No data found'}
          </Typography>
        </Box>
      )
    }
  },
  {
    flex: 0.15,
    minWidth: 200,
    headerName: 'GrantStatus',
    field: 'GrantStatus',
    renderCell: ({ row }) => {
      return (
        <Box sx={{ display: 'flex', alignItems: 'center' }}>
          <Typography noWrap sx={{ color: 'text.secondary', textTransform: 'capitalize' }}>
            {'No data found'}
          </Typography>
        </Box>
      )
    }
  },

  {
    flex: 0.15,
    minWidth: 100,
    sortable: false,
    field: 'actions',
    headerName: 'Actions',
    renderCell: ({ row }) => <RowOptions id={row.id} />
  }
]

const BasicModal = ({ apiData }) => {
  const [open, setOpen] = React.useState(false)

  const handleOpen = () => setOpen(true)

  const handleClose = () => setOpen(false)

  // ** State
  const [role, setRole] = useState('')
  const [plan, setPlan] = useState('')

  const [pageSize, setPageSize] = useState(10)
  const [addUserOpen, setAddUserOpen] = useState(false)

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
  //   console.log(getdata, 'response')

  useEffect(() => {
    viewData()
  }, [])

  return (
    <div>
      <Typography onClick={handleOpen}>Grant Access</Typography>
      <Modal
        open={open}
        sx={modalWrapper}
        onClose={handleClose}
        aria-labelledby='modal-modal-title'
        aria-describedby='modal-modal-description'
      >
        <Box sx={modalBlock}>
          <Box sx={modalContentStyle}>
            <DialogContent>
              <DialogTitle variant='h6'>Grant Access</DialogTitle>

              <IconButton
                aria-label='close'
                onClick={handleClose}
                sx={{ top: 30, right: 10, position: 'absolute', color: 'grey.500' }}
              >
                <Icon icon='mdi:close' />
              </IconButton>
              <Divider />
              <br />
              <Grid container spacing={4}>
                {/* Calling Add user Button */}
                <Adduser />

                <Grid item xs={12}>
                  <DataGrid
                    autoHeight
                    rows={getdata}
                    columns={columns}
                    pageSize={pageSize}
                    disableSelectionOnClick
                    rowsPerPageOptions={[10, 25, 50]}
                    onPageSizeChange={newPageSize => setPageSize(newPageSize)}
                    getRowId={row => row.Id}
                  />
                </Grid>

                <AddUserDrawer open={addUserOpen} toggle={toggleAddUserDrawer} />
              </Grid>
            </DialogContent>
          </Box>
        </Box>
      </Modal>
    </div>
  )
}

export default BasicModal
