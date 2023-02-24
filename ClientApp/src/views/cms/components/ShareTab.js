import * as React from 'react'
import Box from '@mui/material/Box'
import Tab from '@mui/material/Tab'
import TabContext from '@mui/lab/TabContext'
import TabList from '@mui/lab/TabList'
import TabPanel from '@mui/lab/TabPanel'
import InputLabel from '@mui/material/InputLabel'
import MenuItem from '@mui/material/MenuItem'
import FormControl from '@mui/material/FormControl'
import Select from '@mui/material/Select'
import Button from '@mui/material/Button'
import { Typography } from '@mui/material'

export default function ShareTab() {
  const [value, setValue] = React.useState('1')
  const handleChange = (event, newValue) => {
    setValue(newValue)
  }

  const [user, setUser] = React.useState('')
  const handleChangeUser = event => {
    setUser(event.target.value)
  }

  const [team, setTeam] = React.useState('')
  const handleChangeTeam = event => {
    setTeam(event.target.value)
  }

  return (
    <Box sx={{ width: '100%', typography: 'body1' }}>
      <TabContext value={value}>
        <Box sx={{ borderBottom: 1, borderColor: 'divider' }}>
          <TabList onChange={handleChange} aria-label='lab API tabs example'>
            <Tab label='Share With User' value='1' />
            <Tab label='Share With Team' value='2' />
          </TabList>
        </Box>
        <TabPanel value='1'>
          <Typography>Select User To Share</Typography>{' '}
          <Box sx={{display:"flex", justifyContent:"space-between", mt:"10px"}}>
            <FormControl sx={{ width: '32rem' }}>
              <InputLabel id='demo-simple-select-label'>User</InputLabel>
              <Select
                labelId='demo-simple-select-label'
                id='demo-simple-select'
                value={user}
                label='User'
                onChange={handleChangeUser}
              >
                <MenuItem value={10}>Ten</MenuItem>
                <MenuItem value={20}>Twenty</MenuItem>
                <MenuItem value={30}>Thirty</MenuItem>
              </Select>
            </FormControl>
            <Button variant='contained' sx={{width:"6rem"}}>Share</Button>
          </Box>
        </TabPanel>
        <TabPanel value='2'>
          <Typography> Select Team To Share</Typography>
          <Box sx={{display:"flex", justifyContent:"space-between", mt:"10px"}}>
            <FormControl sx={{ width: '32rem' }}>
              <InputLabel id='demo-simple-select-label'>Team</InputLabel>
              <Select
                labelId='demo-simple-select-label'
                id='demo-simple-select'
                value={team}
                label='Team'
                onChange={handleChangeTeam}
              >
                <MenuItem value={10}>Ten</MenuItem>
                <MenuItem value={20}>Twenty</MenuItem>
                <MenuItem value={30}>Thirty</MenuItem>
              </Select>
            </FormControl>
            <Button variant='contained' sx={{width:"6rem"}}>Share</Button>
          </Box>
        </TabPanel>
      </TabContext>
    </Box>
  )
}
