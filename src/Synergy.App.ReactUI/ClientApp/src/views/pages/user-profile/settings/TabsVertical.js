// ** React Imports
import { useState } from 'react'

// ** MUI Imports
import Box from '@mui/material/Box'
import Tab from '@mui/material/Tab'
import TabList from '@mui/lab/TabList'
import TabPanel from '@mui/lab/TabPanel'
import TabContext from '@mui/lab/TabContext'
import Typography from '@mui/material/Typography'

import Switch from '@mui/material/Switch'

import Button from '@mui/material/Button'
import Stack from '@mui/material/Stack'

import TextField from '@mui/material/TextField'
const label = { inputProps: { 'aria-label': 'Switch demo' } }

const TabsVertical = () => {
  // ** State
  const [value, setValue] = useState('1')

  const handleChange = (event, newValue) => {
    setValue(newValue)
  }

  return (
    <TabContext value={value}>
      <Box sx={{ display: 'flex', height: '700px', width: '700px' }}>
        <TabList orientation='vertical' onChange={handleChange} aria-label='vertical tabs example'>
          <Tab value='1' label='profile' />
          <Tab value='2' label='Preferences' />
        </TabList>
        <TabPanel value='1'>
          <Typography variant='h5' component='h5'>
            Administrator
          </Typography>
          <br/>
          <img
            src={`https://synergydev.aitalkx.com/Cms/document/getimagemongo/92133c2a-6a6c-4422-989f-c900eae6992e`}
            alt={'Image'}
            loading='lazy'
            height='350px'
            width='350px'
          />
          <br />
          Name
          <TextField fullWidth label=' Administrator' id='outlined-full-width' sx={{ mb: 4 }} />
          job Title
          <TextField fullWidth label='System Administrator' id='outlined-full-width' sx={{ mb: 4 }} />
          <hr />
          <Stack direction='row' alignItems='center' spacing={2} sx={{display:"flex",justifyContent:'right'}}>
            <Button variant='contained' component='label'>
              Upload Profile
              {/* <input hidden accept='image/*' multiple type='file' /> */}
            </Button>
          </Stack>
        </TabPanel>
        <TabPanel value='2'>
          <Typography>Notification</Typography>
          <br/>
          <Typography variant='h5' component='h5'>
            Enable regular email
            <Switch {...label} defaultChecked />
          </Typography>
          <br/>
          <Typography variant='h5' component='h5'>
            Enable regular email
            <Switch {...label} defaultChecked />
          </Typography>
          
          <hr />
          <Stack direction='row' alignItems='center' spacing={2} sx={{display:"flex",justifyContent:'right'}}>
            <Button variant='contained' component='label' >
              Upload Preferences
              {/* <input hidden accept='image/*' multiple type='file' /> */}
            </Button>
          </Stack>
        </TabPanel>
      </Box>
    </TabContext>
  )
}
export default TabsVertical
