import * as React from 'react'
//import { Icon } from '@iconify/react';

import Box from '@mui/material/Box'

import Button from '@mui/material/Button'

import Typography from '@mui/material/Typography'
import Icon from 'src/@core/components/icon'

import Modal from '@mui/material/Modal'

import TextField from '@mui/material/TextField'
import { Divider, FormControl, InputLabel, Select, MenuItem, value } from '@mui/material'
import MenuButton from './MenuButton'
import Comment from './Comment'
// import MenuBtn from './MenuBtn'

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

  width: '50rem',

  mb: 3,

  borderRadius: '10px'
}

export default function OtherReimbModal() {
  const [value, setValue] = React.useState('')
  const handleChanges = event => {
    setValue(event.target.value)
  }
  const [open, setOpen] = React.useState(false)

  const handleOpen = () => setOpen(true)

  const handleClose = () => setOpen(false)

  return (
    <div>
      <Button variant='contained' onClick={handleOpen}>
        Create
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
              <Typography sx={{ p: 4 }} variant='h4' component='h3'>
                Remote SignIn SignOut
              </Typography>

              <Box sx={{ display: 'flex', justifyContent: 'space-between', alignContent: 'center' }}>
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
            <hr />
            <Box sx={{ py: 3, px: 5 }}>
              <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
                <Typography sx={{ p: 4 }} variant='h6' component='h3'>
                  Remote SignIn SignOut
                </Typography>

                <Box sx={{ display: 'flex' }}>
                  <Button sx={{ borderRadius: '50px', display: 'flex', flexDirection: 'column' }} component='label'>
                    <Comment />
                    <input type='Text' hidden />
                  </Button>
                  <Box>
                    <MenuButton />
                  </Box>

                  <Button sx={{ borderRadius: '50px', display: 'flex', flexDirection: 'column' }} component='label'>
                    <Icon icon='mdi:attachment-plus' />

                    <input type='file' hidden />
                  </Button>
                  <Button
                    sx={{ borderRadius: '50px', display: 'flex', flexDirection: 'column' }}
                    onClick={handleClose}
                    component='label'
                  >
                    <Icon icon='mdi:close' />
                  </Button>
                </Box>
              </Box>
              <hr />
              <Box required fullWidth sx={{ display: 'flex', alignItems: 'center', mt: 10, mb: 5, gap: 10 }}>
                <Typography sx={{ p: '2px', borderRadius: '5px' }}>
                  <Button component='label'>
                    <Icon icon='ic:outline-perm-identity' />
                  </Button>
                  Administratoradminsynergy.com
                </Typography>
                <Typography sx={{ p: '2px', borderRadius: '5px' }}>S-14-02-2023-19</Typography>
                <Typography sx={{ p: '2px', borderRadius: '5px' }}>Draft</Typography>
                <Typography sx={{ p: '2px', borderRadius: '5px' }}>Tomorow</Typography>
                <Typography sx={{ p: '2px', borderRadius: '5px' }}>V1</Typography>
              </Box>

              <Box sx={{ display: 'flex', mt: 15, mb: 5, justifyContent: 'flex-end', gap: 3 }}>
                <Button variant='contained'>Save As Draft</Button>
                <Button variant='contained'>Submit</Button>
              </Box>

              <Box>
                <TextField
                  required
                  fullWidth
                  sx={{ marginY: '5px' }}
                  id='date'
                  label='SignOut Time'
                  type='date'
                  defaultValue='YYYY-MM-DD'
                  InputLabelProps={{
                    shrink: true
                  }}
                />
                <TextField
                  required
                  fullWidth
                  sx={{ marginY: '5px' }}
                  id='date'
                  label='SignIn Time'
                  type='date'
                  defaultValue='YYYY-MM-DD'
                  InputLabelProps={{
                    shrink: true
                  }}
                />{' '}
                <Box sx={{ px: 4, py: 5, display: 'flex', justifyContent: 'space-between' }}>
                  <FormControl required fullWidth>
                    <InputLabel required id='demo-simple-select-label' sx={{ width: '800px' }}>
                      Location
                    </InputLabel>
                    <Select
                      labelId='demo-simple-select-label'
                      id='demo-simple-select'
                      value={value}
                      label='Location'
                      onChange={handleChanges}
                    >
                      <MenuItem value='Abu Dhabi'>Abu Dhabi</MenuItem>
                      <MenuItem value='Sharjah'>Sharjah</MenuItem>
                      <MenuItem value='Al Mankhool'>Al Mankhool</MenuItem>
                      <MenuItem value='Satwa'>Satwa</MenuItem>
                      <MenuItem value='Kuwait'>Kuwait</MenuItem>
                      <MenuItem value='Saudi Arabia'>Saudi Arabia</MenuItem>
                      <MenuItem value='Lebanon'>Lebanon</MenuItem>
                      <MenuItem value='Jordan'>Jordan</MenuItem>
                      <MenuItem value='Egypt'>Egypt</MenuItem>
                      <MenuItem value='Bahrain'>Bahrain</MenuItem>
                      <MenuItem value='UAE'>UAE</MenuItem>
                      <MenuItem value='Tunisia'>Tunisia</MenuItem>
                      <MenuItem value='Morocco'>Morocco</MenuItem>
                      <MenuItem value='Bhopal'>Bhopal</MenuItem>
                    </Select>
                  </FormControl>
                </Box>
              </Box>
              <Box sx={{ display: 'flex', mt: 15, mb: 5, justifyContent: 'flex-end', gap: 3 }}>
                <Button variant='contained'>Save As Draft</Button>
                <Button variant='contained'>Submit</Button>
              </Box>
            </Box>
          </Box>
        </Box>
      </Modal>
    </div>
  )
}
