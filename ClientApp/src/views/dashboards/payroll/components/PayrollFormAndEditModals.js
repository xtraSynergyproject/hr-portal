import React from 'react'
import Box from '@mui/material/Box'
import FormControlLabel from '@mui/material/FormControlLabel'
import Switch from '@mui/material/Switch'
import InputLabel from '@mui/material/InputLabel'
import MenuItem from '@mui/material/MenuItem'
import FormControl from '@mui/material/FormControl'
import Select from '@mui/material/Select'
import PayrollSearchBar from './Form/PayrollSearchBar'
import Modal from '@mui/material/Modal'
import { ButtonGroup, Button, Typography, Grid } from '@mui/material'
import PayrollAddEmployeeForm from './Form/PayrollAddEmployeeForm'

// *********************Form Modal Styles*********************************

const modalWrapper = {
  overflow: 'auto',

  maxHeight: '100vh',

  display: 'flex'
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

  boxShadow: 24,

  mt: 3,

  width: '60rem',

  mb: 3,

  borderRadius: '10px'
}

//   ***********************************Edit Modal Styles***********************************

const EditmodalWrapper = {
  overflow: 'auto',

  maxHeight: '100vh',

  display: 'flex'
}

const EditmodalBlock = {
  position: 'relative',

  zIndex: 0,

  display: 'flex',

  alignItems: 'center',

  justifyContent: 'center',

  margin: 'auto'
}

const EditmodalContentStyle = {
  position: 'relative',

  background: '#fff',

  boxShadow: 24,

  mt: 3,

  width: '55rem',

  mb: 3,

  borderRadius: '10px',
  display: 'flex',
  justifyContent: 'space-between'
}

export default function PayrollFormAndEditModals() {
  // ***********************Form Modal Events*******************
  const [open, setOpen] = React.useState(false)
  const handleOpen = () => setOpen(true)
  const handleClose = () => setOpen(false)

  // ********************Edit Modal Events*************************
  const [opens, setOpens] = React.useState(false)
  const handleOpens = () => setOpens(true)
  const handleCloses = () => setOpens(false)
  const [value, setValue] = React.useState('')

  const handleChanges = event => {
    setValue(event.target.value)
  }

  return (
    <Box sx={{ display: 'flex', justifyContent: 'flex-end' }}>
      {/* <Grid container spacing={2}> */}
      <Box>
        <Button onClick={handleOpen} sx={{ width: '170px', mr: 2 }} variant='contained'>
          Add Employee
        </Button>

        <Modal
          open={open}
          sx={modalWrapper}
          onClose={handleClose}
          aria-labelledby='modal-modal-title'
          aria-describedby='modal-modal-description'
        >
          <Box sx={modalBlock}>
            <Box sx={modalContentStyle}>
              <PayrollAddEmployeeForm />
            </Box>
          </Box>
        </Modal>
      </Box>

      <Box>
        <Button variant='contained' onClick={handleOpens}>
          Edit
        </Button>

        <Modal
          open={opens}
          sx={EditmodalWrapper}
          onClose={handleCloses}
          aria-labelledby='modal-modal-title'
          aria-describedby='modal-modal-description'
        >
          <Box sx={EditmodalBlock}>
            <Box sx={EditmodalContentStyle}>
              <Box sx={{ mx: 3, my: 1.5, width: '50%' }}>
                <FormControl fullWidth sx={{ my: 1.5 }}>
                  <InputLabel id='demo-simple-select-label'>Person</InputLabel>
                  <Select
                    labelId='demo-simple-select-label'
                    id='demo-simple-select'
                    value={value}
                    label='Person'
                    onChange={handleChanges}
                    required
                  >
                    <MenuItem>
                      {' '}
                      <PayrollSearchBar />{' '}
                    </MenuItem>
                    <MenuItem value={10}>Ten</MenuItem>
                    <MenuItem value={20}>Twenty</MenuItem>
                    <MenuItem value={30}>Thirty</MenuItem>
                  </Select>
                </FormControl>

                <FormControl fullWidth sx={{ my: 1.5 }}>
                  <InputLabel id='demo-simple-select-label'>Pay Calender</InputLabel>
                  <Select
                    labelId='demo-simple-select-label'
                    id='demo-simple-select'
                    value={value}
                    label='PayCalender'
                    onChange={handleChanges}
                    required
                  >
                    <MenuItem>
                      {' '}
                      <PayrollSearchBar />{' '}
                    </MenuItem>
                    <MenuItem value={10}>Ten</MenuItem>
                    <MenuItem value={20}>Twenty</MenuItem>
                    <MenuItem value={30}>Thirty</MenuItem>
                  </Select>
                </FormControl>

                <FormControl fullWidth sx={{ my: 1.5 }}>
                  <InputLabel id='demo-simple-select-label'>Pay Group</InputLabel>
                  <Select
                    labelId='demo-simple-select-label'
                    id='demo-simple-select'
                    value={value}
                    label='PayGroup'
                    onChange={handleChanges}
                    required
                  >
                    <MenuItem>
                      {' '}
                      <PayrollSearchBar />{' '}
                    </MenuItem>
                    <MenuItem value={10}>Ten</MenuItem>
                    <MenuItem value={20}>Twenty</MenuItem>
                    <MenuItem value={30}>Thirty</MenuItem>
                  </Select>
                </FormControl>

                <FormControl fullWidth sx={{ my: 1.5 }}>
                  <InputLabel id='demo-simple-select-label'>Payment Mode</InputLabel>
                  <Select
                    labelId='demo-simple-select-label'
                    id='demo-simple-select'
                    value={value}
                    label='PaymentMode'
                    onChange={handleChanges}
                    required
                  >
                    <MenuItem>
                      {' '}
                      <PayrollSearchBar />{' '}
                    </MenuItem>
                    <MenuItem value={10}>Ten</MenuItem>
                    <MenuItem value={20}>Twenty</MenuItem>
                    <MenuItem value={30}>Thirty</MenuItem>
                  </Select>
                </FormControl>

                <FormControl fullWidth sx={{ my: 1.5 }}>
                  <InputLabel id='demo-simple-select-label'>Flight Ticket Frequency</InputLabel>
                  <Select
                    labelId='demo-simple-select-label'
                    id='demo-simple-select'
                    value={value}
                    label='FlightTicketFrequency'
                    onChange={handleChanges}
                  >
                    <MenuItem>
                      {' '}
                      <PayrollSearchBar />{' '}
                    </MenuItem>
                    <MenuItem value={10}>Ten</MenuItem>
                    <MenuItem value={20}>Twenty</MenuItem>
                    <MenuItem value={30}>Thirty</MenuItem>
                  </Select>
                </FormControl>

                <FormControl fullWidth sx={{ my: 1.5 }}>
                  <InputLabel id='demo-simple-select-label'>Overtime Payment Type</InputLabel>
                  <Select
                    labelId='demo-simple-select-label'
                    id='demo-simple-select'
                    value={value}
                    label='OvertimePaymentType'
                    onChange={handleChanges}
                  >
                    <MenuItem>
                      {' '}
                      <PayrollSearchBar />{' '}
                    </MenuItem>
                    <MenuItem value={10}>Ten</MenuItem>
                    <MenuItem value={20}>Twenty</MenuItem>
                    <MenuItem value={30}>Thirty</MenuItem>
                  </Select>
                </FormControl>
              </Box>

              <Box sx={{ mt: 3, mr: 3 }}>
                <Box>
                  <FormControlLabel
                    label='Take Attendance From TAA
'
                    control={<Switch />}
                  />{' '}
                  <br />
                  <FormControlLabel
                    label='  Is Employee Eligible For Overtime
'
                    control={<Switch />}
                  />{' '}
                  <br />
                  <FormControlLabel
                    label='  Is Employee Eligible For Flight Tickets For Self
'
                    control={<Switch />}
                  />{' '}
                  <br />
                  <FormControlLabel label='Is Validate Dependent Document For Benefit' control={<Switch />} /> <br />
                  <FormControlLabel
                    label='  Is Employee Eligible For End Of Service
'
                    control={<Switch />}
                  />{' '}
                  <br />
                  <FormControlLabel
                    label='Is Employee Eligible For Flight Tickets For Dependants'
                    control={<Switch />}
                  />{' '}
                  <br />
                  <FormControlLabel
                    label='  Disable Flight Ticket Processing In Payroll
'
                    control={<Switch />}
                  />{' '}
                  <br />
                  <FormControlLabel
                    label='  Is Eligible For Salary Transfer Letter
'
                    control={<Switch />}
                  />
                </Box>
                <ButtonGroup sx={{ float: 'right', my: 3 }}>
                  <Button variant='outlined' size='medium' onClick={handleCloses}>
                    Cancel
                  </Button>
                  <Button variant='contained' size='medium'>
                    Submit
                  </Button>{' '}
                </ButtonGroup>
              </Box>
            </Box>
          </Box>
        </Modal>
      </Box>
      {/* </Grid> */}
    </Box>
  )
}
