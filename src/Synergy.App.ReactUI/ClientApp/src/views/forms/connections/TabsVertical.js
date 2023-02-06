// ** React Imports
import { useState } from 'react'

// ** MUI Imports
import Box from '@mui/material/Box'
import Tab from '@mui/material/Tab'
import TabList from '@mui/lab/TabList'
import TabPanel from '@mui/lab/TabPanel'
import TabContext from '@mui/lab/TabContext'
import Typography from '@mui/material/Typography'

import Switch from '@mui/material/Switch';
const label = { inputProps: { 'aria-label': 'Switch demo' } };

const TabsVertical = () => {
  // ** State
  const [value, setValue] = useState('1')

  const handleChange = (event, newValue) => {
    setValue(newValue)
  }

  return (
    <TabContext value={value}>
      <Box sx={{ display: 'flex',height:'300px',width:'700px'}}>
        <TabList orientation='vertical' onChange={handleChange} aria-label='vertical tabs example'>
          <Tab value='1' label='profile' />
          <Tab value='2' label='Preferences' />
        </TabList>
        <TabPanel value='1'>
          <Typography variant="h5" component="h5">
           Administrator
           {/* <Switch {...label} defaultChecked /> */}
          </Typography>
        </TabPanel>
        <TabPanel value='2' sx={{display:'flex',alignItems:'flex-start',flexDirection:'column',p:1}}>
        <Typography >
            Notification
          </Typography>
          <Typography variant="h5" component="h5">
            Enable regular email
           <Switch {...label} defaultChecked />
          </Typography>
          <Typography variant="h5" component="h5">
            Enable regular email
           <Switch {...label} defaultChecked />
          </Typography>
        </TabPanel>
      </Box>
    </TabContext>
  )
}

export default TabsVertical
