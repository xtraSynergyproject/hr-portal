// ** React Imports
// import { useState } from 'react'

// ** MUI Imports
import Tab from '@mui/material/Tab'
import Card from '@mui/material/Card'
import TabList from '@mui/lab/TabList'
import TabPanel from '@mui/lab/TabPanel'
import Button from '@mui/material/Button'
import TabContext from '@mui/lab/TabContext'
// import Typography from '@mui/material/Typography'
import CardContent from '@mui/material/CardContent'
// // ** MUI Components
import Box from '@mui/material/Box'
import Grid from '@mui/material/Grid'
// ** Custom Components Imports
import { Paper, Typography } from '@mui/material'
import Cards1 from './Cards1'
import Cards2 from './Cards2'
import Cards3 from './Cards3'
import Cards4 from './Cards4'
import { useState, useEffect } from 'react'
import axios from 'axios'
import {Divider} from '@mui/material'

function createData(DraftCount, InProgressCount, CompletedCount, CanceledCount) {
  return createData(DraftCount, InProgressCount, CompletedCount, CanceledCount)
}

const CardNavigation = () => {
  // ** State
  const [value, setValue] = useState('1')

  const handleChange = (event, newValue) => {
    setValue(newValue)
  }
  // Api intergration by using get method
  const [getdata, setGetdata] = useState([])
  const viewData = async () => {
    let response = await axios.get(
      `https://webapidev.aitalkx.com/cms/task/GetTaskSummary?userId=45bba746-3309-49b7-9c03-b5793369d73c&portalName=HR`
    )
    setGetdata(response.data)
    console.log(response.data, 'cardsdata')
  }

  console.log(getdata, 'response')
  useEffect(() => {
    viewData()
  }, [])
  return (
    <Card > 
      <CardContent>
        <Grid sx={{height:'auto'}} container spacing={12}>
          <Grid className='UDDmaingrid' item xs={12} sx={{ display: 'flex', justifyContent: 'space-between' }}>
              <Card>
                <Typography fontSize='15px'>
                  <b>Draft</b>
                  <b>({getdata.DraftCount})</b>
                </Typography>
                <Card
                  elevation={1}
                  sx={{ height: '80vh', width: '17vw', overflow: 'scroll', backgroundColor: '#f0f0f0' }}
                >
                  <Cards1 />
                  <Cards1 />
                  <Cards1 />
                  <Cards1 />
                  <Cards1 />
                </Card>
              </Card>
              <Card>
                <Typography fontSize='15px'>
                  <b>In Progress</b>
                  <b>({getdata.InProgressCount})</b>
                </Typography>
                <Card
                  elevation={1}
                  sx={{ height: '80vh', width: '17vw', overflow: 'scroll', backgroundColor: '#f0f0f0' }}
                >
                  <Cards2 />
                  <Cards2 />
                  <Cards2 />
                  <Cards2 />
                  <Cards2 />
                </Card>
              </Card>

              <Card>
                <Typography fontSize='15px'>
                  <b>Completed</b>
                  <b>({getdata.CompletedCount})</b>{' '}
                </Typography>
                <Card
                  elevation={1}
                  sx={{ height: '80vh', width: '17vw', overflow: 'scroll', backgroundColor: '#f0f0f0' }}
                >
                  <Cards3 />
                  <Cards3 />
                  <Cards3 />
                  <Cards3 />
                  <Cards3 />
                </Card>
              </Card>

              <Card>
                <Typography fontSize='15px'>
                  <b>Cancelled</b>
                  <b>({getdata.CanceledCount})</b>
                </Typography>
                <Card
                  elevation={1}
                  sx={{ height: '80vh', width: '17vw', overflow: 'scroll', backgroundColor: '#f0f0f0' }}
                >
                  <Cards4 />
                  <Cards4 />
                  <Cards4 />
                  <Cards4 />
                  <Cards4 />
                </Card>
              </Card>
          </Grid>
        </Grid>
      </CardContent>
    </Card>
  )
}

export default CardNavigation
