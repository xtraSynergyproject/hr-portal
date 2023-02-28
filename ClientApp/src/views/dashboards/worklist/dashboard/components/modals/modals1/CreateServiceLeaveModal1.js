import React from 'react'
import Box from '@mui/material/Box'
import Button from '@mui/material/Button'
import TextField from '@mui/material/TextField'
import Typography from '@mui/material/Typography'
import Modal from '@mui/material/Modal'
import Icon from 'src/@core/components/icon'
// import MenuBtn from './MenuBtn'
import { Card, Divider } from '@mui/material'
import Select from '@mui/material/Select'
import MenuItem from '@mui/material/MenuItem'
import InputLabel from '@mui/material/InputLabel'
import FormControl from '@mui/material/FormControl'
// import MoreDetailsModal from './MoreDetailsModal'

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

function CreateServiceLeaveModal() {
  const [open, setOpen] = React.useState(false)
  const handleOpen = () => setOpen(true)
  const handleClose = () => setOpen(false)

  return (
    <Box>
      <Button variant='contained' sx={{textTransform:'capitalize'}}onClick={handleOpen}>
        +Create Service
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
            <Box sx={{ mb: 2, display: 'flex', justifyContent: 'space-between' }} className='demo-space-x'>
              <Typography sx={{ p: 4 }} variant='h5' component='h3'>
                Add Misconducts
              </Typography>

              <Box sx={{ display: 'flex', justifyContent: 'space-between', alignContent: 'center' }}>
                <Button sx={{ borderRadius: '50px', display: 'flex', flexDirection: 'column' }} component='label'>
                  <Icon icon='mdi:attachment-plus' />
                  Attachment
                  <input type='file' hidden />
                </Button>

                {/* <MenuBtn /> */}

                <Button
                  sx={{ borderRadius: '50px', display: 'flex', flexDirection: 'column' }}
                  onClick={handleClose}
                  component='label'
                >
                  <Icon icon='mdi:close' />
                  Close
                </Button>
              </Box>
            </Box>
            <Divider />

            <Box sx={{ mx: 4 }}>
              <Box>
              <Box sx={{ p: 3 }}>
                    <Typography sx={{ my: 3 }}>
                      Service Owner/Requester: <b>{}</b>
                    </Typography>
                    <Typography sx={{ my: 3 }}>
                      Service Number: <b>{}</b>
                    </Typography>
                    <Typography sx={{ my: 3 }}>
                      Service Status: <b>{}</b>
                    </Typography>
                    <Typography sx={{ my: 3 }}>
                      Due Date: <b>{}</b>
                    </Typography>
                    <Typography sx={{ my: 3 }}>
                      Service Version: <b>{}</b>
                    </Typography>
                  </Box>
                <Box sx={{ display: 'flex', gap: 3 }}>
                  <Box>
                    <TextField
                      id='outlined-basic'
                      fullWidth
                      label='Subject'
                      variant='outlined'
                      sx={{ mb: 1.5, mt: 3 }}
                    />

                    <FormControl fullWidth sx={{ my: 1.5 }}>
                      <InputLabel required fullWidth id='demo-simple-select-helper-label'>
                        Misconduct Type
                      </InputLabel>
                      <Select
                        label='Misconduct Type    '
                        defaultValue=''
                        id='demo-simple-select-helper'
                        labelId='demo-simple-select-helper-label'
                      >
                        <MenuItem value=''>
                          <em>None</em>
                        </MenuItem>
                        <MenuItem value={10}>Ten</MenuItem>
                        <MenuItem value={20}>Twenty</MenuItem>
                        <MenuItem value={30}>Thirty</MenuItem>
                      </Select>
                    </FormControl>

                    <FormControl fullWidth sx={{ my: 1.5 }}>
                      <InputLabel required id='demo-simple-select-helper-label'>
                        Fine Amount Type
                      </InputLabel>
                      <Select
                        required
                        label='Fine Amount Type'
                        defaultValue=''
                        id='demo-simple-select-helper'
                        labelId='demo-simple-select-helper-label'
                      >
                        <MenuItem value=''>
                          <em>None</em>
                        </MenuItem>
                        <MenuItem value={10}>Ten</MenuItem>
                        <MenuItem value={20}>Twenty</MenuItem>
                        <MenuItem value={30}>Thirty</MenuItem>
                      </Select>
                    </FormControl>

                    <TextField
                      type='number'
                      id='outlined-basic'
                      fullWidth
                      label='Amount'
                      variant='outlined'
                      sx={{ mt: 1.5, mb: 3 }}
                    />
                  </Box>

                  <Box>
                    <TextField
                      required
                      fullWidth
                      sx={{ mb: 1.5, mt: 3 }}
                      id='date'
                      label='Misconduct Date'
                      type='date'
                      defaultValue='YYYY-MM-DD'
                      InputLabelProps={{
                        shrink: true
                      }}
                    />

                    <FormControl fullWidth sx={{ my: 1.5 }}>
                      <InputLabel required id='demo-simple-select-helper-label'>
                        Disciplinary Action Taken
                      </InputLabel>
                      <Select
                        required
                        label='Disciplinary Action Taken'
                        defaultValue=''
                        id='demo-simple-select-helper'
                        labelId='demo-simple-select-helper-label'
                      >
                        <MenuItem value=''>
                          <em>None</em>
                        </MenuItem>
                        <MenuItem value={10}>Ten</MenuItem>
                        <MenuItem value={20}>Twenty</MenuItem>
                        <MenuItem value={30}>Thirty</MenuItem>
                      </Select>
                    </FormControl>

                    <FormControl fullWidth sx={{ my: 1.5 }}>
                      <InputLabel required id='demo-simple-select-helper-label'>
                        {' '}
                        User
                      </InputLabel>
                      <Select
                        required
                        label='User'
                        defaultValue=''
                        id='demo-simple-select-helper'
                        labelId='demo-simple-select-helper-label'
                      >
                        <MenuItem value=''>
                          <em>None</em>
                        </MenuItem>
                        <MenuItem value={10}>Ten</MenuItem>
                        <MenuItem value={20}>Twenty</MenuItem>
                        <MenuItem value={30}>Thirty</MenuItem>
                      </Select>
                    </FormControl>

                    <TextField
                      type='number'
                      id='outlined-basic'
                      fullWidth
                      label='No. Of Days'
                      variant='outlined'
                      sx={{ mt: 1.5, mb: 3 }}
                    />
                  </Box>
                </Box>

                <TextField id='outlined-basic' fullWidth label='Desription' multiline rows={4} variant='outlined' />
              </Box>

              <Box>
                <Typography variant='h6' component='p' sx={{ mt: 10, mb: 3 }}>
                  Other Information
                </Typography>
                <Divider />
                <TextField
                  id='outlined-basic'
                  fullWidth
                  label='Investigation Result'
                  multiline
                  rows={3}
                  variant='outlined'
                  sx={{ mt: 3, mb: 1.5 }}
                />
                <TextField
                  id='outlined-basic'
                  fullWidth
                  label='Desription'
                  multiline
                  rows={4}
                  variant='outlined'
                  sx={{ mt: 1.5, mb: 5 }}
                />
              </Box>

              <Box sx={{ display: 'flex', my: 5, justifyContent: 'flex-end', gap: 3 }}>
                {/* <MoreDetailsModal/> */}
                <Button variant='contained'>Save As Draft</Button>
                <Button variant='contained'>Submit</Button>
              </Box>
            </Box>
          </Box>
        </Box>
      </Modal>
    </Box>
  )
}

export default CreateServiceLeaveModal
